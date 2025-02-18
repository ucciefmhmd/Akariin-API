using Application.Services.File;
using Application.Utilities.Models;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Application.Contract.Commends.Update
{
    public record UpdateContractCommand(long Id, string ContractNumber, string PaymentCycle, string AutomaticRenewal, string ContractRent, DateTime DateOfConclusion, DateTime StartDate, DateTime EndDate, string Type, decimal? TenantTax, string Status, IFormFile? ContractFile, bool IsActive, bool IsExecute, bool IsFinished, long RealEstateUnitId, long RealEstateId, long TenantId) : IRequest<UpdateContractCommandResult>;

    public record UpdateContractCommandResult : BaseCommandResult
    {
        public long Id { get; set; }
    }

    public class UpdateContractCommandHandler(ApplicationDbContext _dbContext, AttachmentService _attachmentService) : IRequestHandler<UpdateContractCommand, UpdateContractCommandResult>
    {
        public async Task<UpdateContractCommandResult> Handle(UpdateContractCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var contract = await _dbContext.Contracts.FindAsync(new object[] { request.Id }, cancellationToken);

                if (contract == null)
                {
                    return new UpdateContractCommandResult
                    {
                        IsSuccess = false,
                        Id = request.Id,
                        Errors = { "Contract not found." }
                    };
                }

                contract.StartDate = request.StartDate;
                contract.EndDate = request.EndDate;
                contract.DateOfConclusion = request.DateOfConclusion;
                contract.Status = request.Status;
                contract.TenantTax = request.TenantTax;
                contract.IsActive = request.IsActive;
                contract.IsExecute = request.IsExecute;
                contract.IsFinished = request.IsFinished;
                contract.RealEstateId = request.RealEstateId;
                contract.AutomaticRenewal = request.AutomaticRenewal;
                contract.ContractNumber = request.ContractNumber;
                contract.ContractRent = request.ContractRent;
                contract.PaymentCycle = request.PaymentCycle;
                contract.Type = request.Type;
                contract.RealEstateUnitId = request.RealEstateUnitId;
                contract.TenantId = request.TenantId;
                if (request.ContractFile is not null)
                {
                    // Delete old image if it exists
                    if (!string.IsNullOrEmpty(contract.ContractFile))
                    {
                        await _attachmentService.DeleteFilesAsync(contract.Id.ToString());
                    }

                    // Upload new image
                    await _attachmentService.UploadFilesAsync(Path.Combine("Contracts", request.Id.ToString()), request.ContractFile);

                }



                var validationResults = new List<ValidationResult>();

                var isValid = Validator.TryValidateObject(contract, new ValidationContext(contract), validationResults, true);

                if (!isValid)
                {
                    return new UpdateContractCommandResult
                    {
                        IsSuccess = false,
                        Errors = validationResults.Select(vr => vr.ErrorMessage).ToList(),
                        ErrorCode = Domain.Common.ErrorCode.InvalidDate
                    };
                }

                _dbContext.Contracts.Update(contract);

                await _dbContext.SaveChangesAsync(cancellationToken);

                return new UpdateContractCommandResult
                {
                    IsSuccess = true,
                    Id = contract.Id
                };
            }
            catch (Exception ex)
            {
                return new UpdateContractCommandResult
                {
                    IsSuccess = false,
                    Errors = { ex.Message },
                    ErrorCode = Domain.Common.ErrorCode.Error
                };
            }
        }
    }
}
