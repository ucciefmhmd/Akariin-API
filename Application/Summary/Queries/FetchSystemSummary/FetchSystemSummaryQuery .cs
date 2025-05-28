using Application.Utilities.Models;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Summary.Queries.FetchSystemSummary
{
    public record FetchSystemSummaryQuery : IRequest<FetchSystemSummaryQueryResult>;

    public record FetchSystemSummaryQueryResult : BaseCommandResult
    {
        public CountSummaryDto? Summary { get; init; }
    }

    public record CountSummaryDto
    {
        public int RealEstatesCount { get; init; }
        public int RealEstateUnitsCount { get; init; }
        public int ContractsCount { get; init; }
        public int BillsCount { get; init; }
        public int MaintenanceRequestsCount { get; init; }
        public int TenantsCount { get; init; }
        public int OwnersCount { get; init; }
        public int MarketersCount { get; init; }
    }

    public class GetCountAllDataQueryHandler(ApplicationDbContext _dbContext) : IRequestHandler<FetchSystemSummaryQuery, FetchSystemSummaryQueryResult>
    {
        public async Task<FetchSystemSummaryQueryResult> Handle(FetchSystemSummaryQuery request, CancellationToken cancellationToken)
        {
            try
            {
                CountSummaryDto summary = new()
                {
                    RealEstatesCount = await _dbContext.RealEstates.CountAsync(u => u.IsActive, cancellationToken),
                    RealEstateUnitsCount = await _dbContext.RealEstateUnits.CountAsync(u => u.IsActive, cancellationToken),
                    ContractsCount = await _dbContext.Contracts.CountAsync(u => u.IsActive, cancellationToken),
                    BillsCount = await _dbContext.Bills.CountAsync(u => u.IsActive, cancellationToken),
                    MaintenanceRequestsCount = await _dbContext.MaintenanceRequests.CountAsync(u => u.IsActive, cancellationToken),
                    TenantsCount = await _dbContext.Members.CountAsync(m => m.Role == "Tenant" && m.IsActive , cancellationToken),
                    OwnersCount = await _dbContext.Members.CountAsync(m => m.Role == "RealEstateOwner" && m.IsActive, cancellationToken),
                    MarketersCount = await _dbContext.Members.CountAsync(m => m.Role == "Marketer" && m.IsActive, cancellationToken)
                };

                return new FetchSystemSummaryQueryResult
                {
                    Summary = summary,
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new FetchSystemSummaryQueryResult
                {
                    IsSuccess = false,
                    Errors = [ ex.Message ]
                };
            }
        }
    }
}