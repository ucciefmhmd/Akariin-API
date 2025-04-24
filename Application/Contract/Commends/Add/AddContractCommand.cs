using Application.Bill.Commends.Add;
using Application.Services.File;
using Application.Utilities.Models;
using Domain.Common;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Contract.Commends.Add
{
    public record AddContractCommand(
        string PaymentCycle, string AutomaticRenewal, string ContractRent, DateTime DateOfConclusion,
        DateTime StartDate, DateTime EndDate, string Type, decimal? TenantTax, string Status, IFormFile? ContractFile,
        bool IsExecute, bool IsFinished, long RealEstateUnitId, long RealEstateId,
        long TenantId, long MarketerId, decimal PaymentAmount
    ) : IRequest<AddContractCommandResult>;

    public record AddContractCommandResult : BaseCommandResult
    {
        public long Id { get; set; }
    }

    public class AddContractCommandHandler(ApplicationDbContext _dbContext, AttachmentService _attachmentService)
        : IRequestHandler<AddContractCommand, AddContractCommandResult>
    {
        public async Task<AddContractCommandResult> Handle(AddContractCommand request, CancellationToken cancellationToken)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                var tenant = await _dbContext.Members.FindAsync([request.TenantId], cancellationToken: cancellationToken);

                if (tenant == null)
                {
                    return new AddContractCommandResult
                    {
                        IsSuccess = false,
                        Errors = { "Tenant not found." },
                        ErrorCode = ErrorCode.NotFound
                    };
                }

                var marketer = await _dbContext.Members.FindAsync([request.MarketerId], cancellationToken: cancellationToken);

                if (marketer == null)
                {
                    return new AddContractCommandResult
                    {
                        IsSuccess = false,
                        Errors = { "Marketer not found." },
                        ErrorCode = ErrorCode.NotFound
                    };
                }


                var contract = new Domain.Models.Contracts.Contract
                {
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    DateOfConclusion = request.DateOfConclusion,
                    Type = request.Type,
                    ContractRent = request.ContractRent,
                    PaymentCycle = request.PaymentCycle,
                    Status = request.Status,
                    TenantTax = request.TenantTax,
                    IsExecute = request.IsExecute,
                    IsFinished = request.IsFinished,
                    PaymentAmount = request.PaymentAmount,
                    RealEstateId = request.RealEstateId,
                    AutomaticRenewal = request.AutomaticRenewal,
                    RealEstateUnitId = request.RealEstateUnitId,
                    TenantId = request.TenantId,
                    MarketerId = request.MarketerId,
                    IsActive = true,
                    IsDeleted = false
                };

                await _dbContext.Contracts.AddAsync(contract, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);

                int numberOfPayments = request.PaymentCycle switch
                {
                    "annual" => 1,
                    "semiAnnual" => 2,
                    "fourMonths" => 3,
                    "threeMonths" => 4,
                    "monthly" => 12,
                    _ => 1
                };

                List<Domain.Models.Bills.Bill> bills = [];

                DateTime billDate = request.StartDate;

                var taxAmount = (request.TenantTax ?? 0) / 100m;

                for (int i = 0; i < numberOfPayments; i++)
                {
                    if (billDate > request.EndDate)
                        break;

                    bills.Add(new Domain.Models.Bills.Bill
                    {
                        BillDate = billDate,
                        BillNumber = Guid.NewGuid().ToString()[..8],
                        IssuedBy = request.MarketerId.ToString(),
                        StatusBills = StatusBills.Pending,
                        TenantId = request.TenantId,
                        MarketerId = request.MarketerId,
                        Salary = request.PaymentAmount,
                        Discount = 0,
                        Tax = taxAmount,
                        TotalAmount = (request.PaymentAmount * taxAmount),
                        ContractId = contract.Id,
                        IsActive = true,
                        IsDeleted = false
                    });

                    billDate = billDate.AddMonths(12 / numberOfPayments);
                }

                await _dbContext.Bills.AddRangeAsync(bills, cancellationToken);

                await _dbContext.SaveChangesAsync(cancellationToken);

                if (request.ContractFile is not null)
                {
                    await _attachmentService.UploadFilesAsync(Path.Combine("Contracts", contract.Id.ToString()), request.ContractFile);
                }

                await transaction.CommitAsync(cancellationToken);

                return new AddContractCommandResult
                {
                    IsSuccess = true,
                    Id = contract.Id
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);

                return new AddContractCommandResult
                {
                    IsSuccess = false,
                    Errors = { ex.Message },
                    ErrorCode = Domain.Common.ErrorCode.Error
                };
            }
        }
    }
}
