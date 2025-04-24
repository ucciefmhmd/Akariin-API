using Application.RealEstate.Queries.GetAllRealEstate;
using Application.Utilities.Extensions;
using Application.Utilities.Filter;
using Application.Utilities.Models;
using Application.Utilities.Sort;
using Domain.Common;
using Infrastructure;
using MediatR;

namespace Application.MaintenanceRequest.Queries.GetAll
{
    public record GetAllMaintenanceRequestQuery : BasePaginatedQuery, IRequest<GetAllMaintenanceRequestQueryResult>
    {
        public string? UserId { get; set; }
    }

    public record GetAllMaintenanceRequestQueryResult : BaseCommandResult
    {
        public BasePaginatedList<MaintenanceRequestDto> dto { get; set; }
    }

    public record MaintenanceRequestDto
    {
        public long Id { get; set; }
        public string RequestNumber { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string CostBearer { get; set; }
        public string MaintenanceType { get; set; }
        public bool IsPrivateMaintenance { get; set; }
        public string? Description { get; set; }
        public string? MaintenanceRequestFile { get; set; }
        public long TenantId { get; set; }
        public long MemberId { get; set; }
        public long RealEstateId { get; set; }
        public long RealEstateUnitId { get; set; }
        public CreatedByVM CreatedBy { get; set; }
        public CreatedByVM ModifiedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }

    public class GetAllMaintenanceRequestQueryHandler(ApplicationDbContext _dbContext) : IRequestHandler<GetAllMaintenanceRequestQuery, GetAllMaintenanceRequestQueryResult>
    {
        public async Task<GetAllMaintenanceRequestQueryResult> Handle(GetAllMaintenanceRequestQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var _maintenanceRequests = await _dbContext.MaintenanceRequests
                    .Search(request.SearchTerm)
                    .Where(mr => !mr.IsDeleted && (mr.CreatedById == request.UserId || request.UserId == null))
                    .Select(mr => new MaintenanceRequestDto
                    {
                        Id = mr.Id,
                        RequestNumber = mr.RequestNumber,
                        RequestDate = mr.RequestDate,
                        DeliveryDate = mr.DeliveryDate,
                        CostBearer = mr.CostBearer,
                        MaintenanceType = mr.MaintenanceType,
                        IsPrivateMaintenance = mr.IsPrivateMaintenance,
                        Description = mr.Description,
                        MaintenanceRequestFile = mr.MaintenanceRequestFile,
                        TenantId = mr.TenantId,
                        MemberId = mr.MemberId,
                        RealEstateId = mr.RealEstateId,
                        RealEstateUnitId = mr.RealEstateUnitId,
                        CreatedBy = mr.CreatedBy != null ? new CreatedByVM { Name = mr.CreatedBy.Name, Id = mr.CreatedBy.Id } : null,
                        ModifiedBy = mr.ModifiedBy != null ? new CreatedByVM { Name = mr.ModifiedBy.Name, Id = mr.ModifiedBy.Id } : null,
                        CreatedDate = mr.CreatedDate,
                        ModifiedDate = mr.ModifiedDate
                    })
                    .Filter(request.Filters)
                    .Sort(request.Sorts ?? new List<SortedQuery>() { new SortedQuery() { PropertyName = "Name", Direction = SortDirection.ASC } })
                    .ToPaginatedListAsync(request.PageNumber, request.PageSize);

                return new GetAllMaintenanceRequestQueryResult
                {
                    IsSuccess = true,
                    dto = _maintenanceRequests
                };
            }
            catch (Exception ex)
            {
                return new GetAllMaintenanceRequestQueryResult
                {
                    IsSuccess = false,
                    ErrorCode = ErrorCode.Error,
                    Errors = new List<string> { ex.Message }
                };
            }
        }
    }
}
