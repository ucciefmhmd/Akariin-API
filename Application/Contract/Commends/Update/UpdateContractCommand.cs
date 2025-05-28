using Application.Services.File;
using Application.Utilities.Models;
using Domain.Common;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Application.Contract.Commends.Update
{
    public record UpdateContractCommand(long Id, string PaymentCycle, string AutomaticRenewal, string ContractRent, DateTime DateOfConclusion, DateTime StartDate, 
        DateTime EndDate, string Type, decimal? TenantTax, string Status, IFormFile? ContractFile, bool IsExecute, decimal AdministrativeExpenses,
        bool IsFinished, long RealEstateUnitId, long RealEstateId, long TenantId, long MarketerId, decimal PaymentAmount) : IRequest<UpdateContractCommandResult>;

    public record UpdateContractCommandResult : BaseCommandResult
    {
        public long Id { get; set; }
    }

    public class UpdateContractCommandHandler(ApplicationDbContext _dbContext, AttachmentService _attachmentService) : IRequestHandler<UpdateContractCommand, UpdateContractCommandResult>
    {
        public async Task<UpdateContractCommandResult> Handle(UpdateContractCommand request, CancellationToken cancellationToken)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                var contract = await _dbContext.Contracts.FindAsync([request.Id], cancellationToken);
                
                if (contract == null)
                {
                    return new UpdateContractCommandResult
                    {
                        IsSuccess = false,
                        Id = request.Id,
                        Errors = { "Contract not found." },
                        ErrorCode = ErrorCode.NotFound
                    };
                }

                var tenant = await _dbContext.Members.FindAsync([request.TenantId], cancellationToken);

                if (tenant == null)
                {
                    return new UpdateContractCommandResult
                    {
                        IsSuccess = false,
                        Errors = { "Tenant not found." },
                        ErrorCode = ErrorCode.NotFound
                    };
                }

                var marketer = await _dbContext.Members.FindAsync([request.MarketerId], cancellationToken);

                if (marketer == null)
                {
                    return new UpdateContractCommandResult
                    {
                        IsSuccess = false,
                        Errors = { "Marketer not found." },
                        ErrorCode = ErrorCode.NotFound
                    };
                }

                contract.StartDate = request.StartDate;
                contract.EndDate = request.EndDate;
                contract.DateOfConclusion = request.DateOfConclusion;
                contract.Status = request.Status;
                contract.TenantTax = request.TenantTax;
                contract.IsExecute = request.IsExecute;
                contract.IsFinished = request.IsFinished;
                contract.AdministrativeExpenses = request.AdministrativeExpenses;
                contract.PaymentAmount = request.PaymentAmount;
                contract.RealEstateId = request.RealEstateId;
                contract.AutomaticRenewal = request.AutomaticRenewal;
                contract.ContractRent = request.ContractRent;
                contract.PaymentCycle = request.PaymentCycle;
                contract.Type = request.Type;
                contract.RealEstateUnitId = request.RealEstateUnitId;
                contract.TenantId = request.TenantId;
                contract.MarketerId = request.MarketerId;
                if (request.ContractFile is not null)
                {
                    if (!string.IsNullOrEmpty(contract.ContractFile))
                    {
                        await _attachmentService.DeleteFilesAsync(contract.Id.ToString());
                    }

                    await _attachmentService.UploadFilesAsync(Path.Combine("Contracts", request.Id.ToString()), request.ContractFile);
                }

                var validationResults = new List<ValidationResult>();

                var isValid = Validator.TryValidateObject(contract, new ValidationContext(contract), validationResults, true);

                if (!isValid)
                {
                    return new UpdateContractCommandResult
                    {
                        IsSuccess = false,
                        Errors = [.. validationResults.Select(vr => vr.ErrorMessage)],
                        ErrorCode = ErrorCode.InvalidDate
                    };
                }

                // Remove old bills
                _dbContext.Bills.RemoveRange(_dbContext.Bills.Where(b => b.ContractId == request.Id));

                int numberOfPayments = request.PaymentCycle switch
                {
                    "annual" => 1,
                    "semiAnnual" => 2,
                    "fourMonths" => 3,
                    "threeMonths" => 4,
                    "monthly" => 12,
                    _ => 1
                };

                var newBills = new List<Domain.Models.Bills.Bill>();

                DateTime billDate = request.StartDate;

                var taxAmount = (request.TenantTax ?? 0) / 100m;

                for (int i = 0; i < numberOfPayments; i++)
                {
                    if (billDate > request.EndDate)
                        break;

                    newBills.Add(new Domain.Models.Bills.Bill
                    {
                        BillDate = billDate,
                        BillNumber = Guid.NewGuid().ToString(),
                        StatusBills = StatusBills.Pending,
                        TenantId = request.TenantId,
                        MarketerId = request.MarketerId,
                        Salary = request.PaymentAmount,
                        Discount = 0,
                        Tax = taxAmount,
                        TotalAmount = request.PaymentAmount * taxAmount,
                        ContractId = contract.Id
                    });

                    billDate = billDate.AddMonths(12 / numberOfPayments);
                }

                await _dbContext.Bills.AddRangeAsync(newBills, cancellationToken);

                await _dbContext.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);

                return new UpdateContractCommandResult
                {
                    IsSuccess = true,
                    Id = contract.Id
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);

                return new UpdateContractCommandResult
                {
                    IsSuccess = false,
                    Errors = { ex.Message },
                    ErrorCode = ErrorCode.Error
                };
            }
        }
    }
}
