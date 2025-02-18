using Application.Services.File;
using Application.Utilities.Models;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Contract.Commends.Add
{
    public record AddContractCommand(string ContractNumber, string PaymentCycle, string AutomaticRenewal, string ContractRent, DateTime DateOfConclusion, DateTime StartDate, DateTime EndDate, string Type, decimal? TenantTax, string Status, IFormFile? ContractFile, bool IsActive, bool IsExecute, bool IsFinished, long RealEstateUnitId, long RealEstateId, long TenantId) : IRequest<AddContractCommandResult>;
    
    public record AddContractCommandResult : BaseCommandResult
    {
        public long Id { get; set; }
    }

    public class AddContractCommandHandler(ApplicationDbContext _dbContext, AttachmentService _attachmentService) : IRequestHandler<AddContractCommand, AddContractCommandResult>
    {
        public async Task<AddContractCommandResult> Handle(AddContractCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var contract = new Domain.Models.Contracts.Contract
                {
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    DateOfConclusion = request.DateOfConclusion,
                    Type = request.Type,
                    ContractNumber = request.ContractNumber,
                    ContractRent = request.ContractRent,
                    PaymentCycle = request.PaymentCycle,
                    Status = request.Status,
                    TenantTax = request.TenantTax,
                    IsActive = request.IsActive,
                    IsExecute = request.IsExecute,
                    IsFinished = request.IsFinished,
                    RealEstateId = request.RealEstateId,
                    AutomaticRenewal = request.AutomaticRenewal,
                    RealEstateUnitId = request.RealEstateUnitId,
                    TenantId = request.TenantId
                };

                await _dbContext.Contracts.AddAsync(contract, cancellationToken);

                await _dbContext.SaveChangesAsync(cancellationToken);

                // Upload the image if provided
                if (request.ContractFile is not null)
                {
                    // Upload new image
                    await _attachmentService.UploadFilesAsync(Path.Combine("Contracts", contract.Id.ToString()), request.ContractFile);
                }

                return new AddContractCommandResult
                {
                    IsSuccess = true,
                    Id = contract.Id
                };
            }
            catch (Exception ex)
            {
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
