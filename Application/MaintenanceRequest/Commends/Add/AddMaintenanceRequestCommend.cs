using Application.Utilities.Models;
using Infrastructure;
using MediatR;

namespace Application.MaintenanceRequest.Commends.Add
{
    public record AddMaintenanceRequestCommend(CreateMaintenanceRequestDto dto) : IRequest<AddMaintenanceRequestCommendResult>;

    public record AddMaintenanceRequestCommendResult : BaseCommandResult
    {
        public long Id { get; set; }
    }

    public record CreateMaintenanceRequestDto
    {
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
    }

    public class AddMaintenanceRequestHandler(ApplicationDbContext _dbContext) : IRequestHandler<AddMaintenanceRequestCommend, AddMaintenanceRequestCommendResult>
    {
        public async Task<AddMaintenanceRequestCommendResult> Handle(AddMaintenanceRequestCommend request, CancellationToken cancellationToken)
        {
            try
            {
                var _maintenanceRequest = new Domain.Models.MaintenanceRequests.MaintenanceRequest
                {
                    RequestNumber = request.dto.RequestNumber,
                    RequestDate = request.dto.RequestDate,
                    DeliveryDate = request.dto.DeliveryDate,
                    CostBearer = request.dto.CostBearer,
                    MaintenanceType = request.dto.MaintenanceType,
                    IsPrivateMaintenance = request.dto.IsPrivateMaintenance,
                    Description = request.dto.Description,
                    MaintenanceRequestFile = request.dto.MaintenanceRequestFile,
                    TenantId = request.dto.TenantId,
                    MemberId = request.dto.MemberId,
                    RealEstateId = request.dto.RealEstateId,
                    RealEstateUnitId = request.dto.RealEstateUnitId
                };

                _dbContext.MaintenanceRequests.Add(_maintenanceRequest);

                await _dbContext.SaveChangesAsync(cancellationToken);

                return new AddMaintenanceRequestCommendResult
                {
                    IsSuccess = true,
                    Id = _maintenanceRequest.Id
                };
            }
            catch (Exception ex)
            {
                return new AddMaintenanceRequestCommendResult
                {
                    IsSuccess = false,
                    Errors = { ex.Message }
                };
            }
        }
    }
}
