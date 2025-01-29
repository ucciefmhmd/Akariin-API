using Application.Contract.Queries.GetAll;
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

    public class GetContractByIdQueryHandler(ApplicationDbContext _dbContext) : IRequestHandler<GetContractByIdQuery, GetContractByIdQueryResult>
    {
        public async Task<GetContractByIdQueryResult> Handle(GetContractByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var contract = await _dbContext.Contracts
                                        .Include(c => c.RealEstateUnit)
                                        .Include(c => c.Tenant)
                                        .Select(c => new ContractDto
                                        {
                                            Id = c.Id,
                                            StartDate = c.StartDate,
                                            EndDate = c.EndDate,
                                            DateOfConclusion = c.DateOfConclusion,
                                            Duration = c.Duration,
                                            Number = c.Number,
                                            Type = c.Type,
                                            TerminationMethod = c.TerminationMethod,
                                            RealEstateUnitId = c.RealEstateUnitId,
                                            TenantId = c.TenantId
                                        }).FirstOrDefaultAsync(cancellationToken);

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
