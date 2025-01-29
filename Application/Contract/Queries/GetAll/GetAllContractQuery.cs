using Application.Utilities.Models;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Domain.Common.Enums.ContractEnum;

namespace Application.Contract.Queries.GetAll
{
    public record GetAllContractQuery : IRequest<GetAllContractQueryResult>;

    public record GetAllContractQueryResult : BaseCommandResult
    {
        public List<ContractDto> dto { get; init; }
    }

    public record ContractDto
    {
        public long Id { get; init; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public DateOnly DateOfConclusion { get; set; }
        public TimeOnly Duration { get; set; }
        public long Number { get; set; }
        public ContractType Type { get; set; }
        public TerminationMethod TerminationMethod { get; set; }
        public long RealEstateUnitId { get; set; }
        public long TenantId { get; set; }
    }

    public class GetAllContractQueryHandler(ApplicationDbContext _dbContext) : IRequestHandler<GetAllContractQuery, GetAllContractQueryResult>
    {
        public async Task<GetAllContractQueryResult> Handle(GetAllContractQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var contracts = await _dbContext.Contracts
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
                                        }).ToListAsync(cancellationToken);

                return new GetAllContractQueryResult
                {
                    IsSuccess = true,
                    dto = contracts
                };
            }
            catch (Exception ex)
            {
                return new GetAllContractQueryResult
                {
                    IsSuccess = false,
                    Errors = new List<string> { ex.Message },
                    ErrorCode = Domain.Common.ErrorCode.Error
                };
            }
        }
    }
}
