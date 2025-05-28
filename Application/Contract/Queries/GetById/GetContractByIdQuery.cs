using Application.Contract.Queries.GetAll;
using Application.RealEstateUnit.Queries.GetAll;
using Application.Services.File;
using Application.Utilities.Models;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Contract.Queries.GetById
{
    public record GetContractByIdQuery(long id) : IRequest<GetContractByIdQueryResult>;

    public record GetContractByIdQueryResult : BaseCommandResult
    {
        public ContractDto dto { get; set; }
    }

    public class GetContractByIdQueryHandler(ApplicationDbContext _dbContext, AttachmentService _attachmentService) : IRequestHandler<GetContractByIdQuery, GetContractByIdQueryResult>
    {
        public async Task<GetContractByIdQueryResult> Handle(GetContractByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var contract = await _dbContext.Contracts
                                        .Include(c => c.RealEstateUnit)
                                        .Include(c => c.Tenant)
                                        .Where(c => !c.IsDeleted && c.Id == request.id)
                                        .Select(c => new ContractDto
                                        {
                                            Id = c.Id,
                                            StartDate = c.StartDate,
                                            EndDate = c.EndDate,
                                            AutomaticRenewal = c.AutomaticRenewal,
                                            ContractNumber = c.ContractNumber,
                                            ContractRent = c.ContractRent,
                                            DateOfConclusion = c.DateOfConclusion,
                                            Type = c.Type,
                                            IsExecute = c.IsExecute,
                                            IsFinished = c.IsFinished,
                                            AdministrativeExpenses = c.AdministrativeExpenses,
                                            PaymentCycle = c.PaymentCycle,
                                            Status = c.Status,
                                            TenantTax = c.TenantTax,
                                            PaymentAmount = c.PaymentAmount,
                                            RealEstateId = c.RealEstateId,
                                            RealEstateUnitId = c.RealEstateUnitId,
                                            MarketerId = c.MarketerId,
                                            MarketerName = c.Marketer.Name,
                                            TenantId = c.TenantId,
                                            TenantName = c.Tenant.Name,
                                            CreatedBy = c.CreatedBy != null ? new CreatedByVM { Name = c.CreatedBy.Name, Id = c.CreatedBy.Id } : null,
                                            ModifiedBy = c.ModifiedBy != null ? new CreatedByVM { Name = c.ModifiedBy.Name, Id = c.ModifiedBy.Id } : null,
                                            CreatedDate = c.CreatedDate,
                                            ModifiedDate = c.ModifiedDate
                                        }).FirstOrDefaultAsync(cancellationToken);

                contract.RealEstateName = await _dbContext.RealEstates
                                                .Where(x => x.Id == contract.RealEstateId)
                                                .Select(x => x.Name)
                                                .FirstOrDefaultAsync(cancellationToken: cancellationToken) ?? string.Empty;

                contract.RealEstateUnitNumber = await _dbContext.RealEstateUnits
                                                .Where(x => x.Id == contract.RealEstateId)
                                                .Select(x => x.UnitNumber)
                                                .FirstOrDefaultAsync(cancellationToken: cancellationToken) ?? string.Empty;

                contract.TenantName = await _dbContext.Members
                                                .Where(x => x.Id == contract.TenantId)
                                                .Select(x => x.Name)
                                                .FirstOrDefaultAsync(cancellationToken: cancellationToken) ?? string.Empty;

                contract.MarketerName = await _dbContext.Members
                                                .Where(x => x.Id == contract.MarketerId)
                                                .Select(x => x.Name)
                                                .FirstOrDefaultAsync(cancellationToken: cancellationToken) ?? string.Empty;

                var _contractFile = await _attachmentService.GetFilesUrlAsync(Path.Combine("contracts", contract.Id.ToString()));

                if (_contractFile.IsSuccess && _contractFile.Urls.Count > 0)
                {
                    contract.ContractFile = _contractFile.Urls[0];
                }
                
                if (contract == null)
                {
                    return new GetContractByIdQueryResult
                    {
                        IsSuccess = false,
                        Errors = new List<string> { "Contract not found." }
                    };
                }

                return new GetContractByIdQueryResult
                {
                    IsSuccess = true,
                    dto = contract
                };

                   
            }
            catch (Exception ex)
            {
                return new GetContractByIdQueryResult
                {
                    IsSuccess = false,
                    Errors = new List<string> { ex.Message },
                    ErrorCode = Domain.Common.ErrorCode.Error
                };
            }
        }
    }
}
