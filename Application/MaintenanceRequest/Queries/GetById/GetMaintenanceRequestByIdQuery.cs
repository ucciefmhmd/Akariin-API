using Application.MaintenanceRequest.Queries.GetAll;
using Application.RealEstate.Queries.GetAllRealEstate;
using Application.Utilities.Models;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.MaintenanceRequest.Queries.GetById
{
    public record GetMaintenanceRequestByIdQuery(long id) : IRequest<GetMaintenanceRequestByIdQueryResult>;

    public record GetMaintenanceRequestByIdQueryResult : BaseCommandResult
    {
        public MaintenanceRequestDto? dto { get; set; }
    }

    public class GetMaintenanceRequestByIdQueryHandler(ApplicationDbContext _dbContext) : IRequestHandler<GetMaintenanceRequestByIdQuery, GetMaintenanceRequestByIdQueryResult>
    {
        public async Task<GetMaintenanceRequestByIdQueryResult> Handle(GetMaintenanceRequestByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var _maintenanceRequest = await _dbContext.MaintenanceRequests
                    .Where(mr => !mr.IsDeleted && mr.Id == request.id)
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
                    }).FirstOrDefaultAsync(cancellationToken);

                if (_maintenanceRequest == null)
                {
                    return new GetMaintenanceRequestByIdQueryResult
                    {
                        IsSuccess = false,
                        Errors = { "Maintenance Request not found." }
                    };
                }
                return new GetMaintenanceRequestByIdQueryResult
                {
                    IsSuccess = true,
                    dto = _maintenanceRequest
                };
            }
            catch (Exception ex)
            {
                return new GetMaintenanceRequestByIdQueryResult
                {
                    IsSuccess = false,
                    Errors = { ex.Message }
                };
            }
        }
    }
}
