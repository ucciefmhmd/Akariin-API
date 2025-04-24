using Application.Utilities.Models;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Application.MaintenanceRequest.Commends.Update
{
    public record class UpdateMaintenanceRequestCommend(UpdateMaintenanceRequestDto dto) : IRequest<UpdateMaintenanceRequestCommendResult>;

    public record class UpdateMaintenanceRequestCommendResult : BaseCommandResult
    {
        public long Id { get; set; }
    }

    public record UpdateMaintenanceRequestDto
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
    }

    public class UpdateMaintenanceRequestCommendHandler : IRequestHandler<UpdateMaintenanceRequestCommend, UpdateMaintenanceRequestCommendResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public UpdateMaintenanceRequestCommendHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UpdateMaintenanceRequestCommendResult> Handle(UpdateMaintenanceRequestCommend request, CancellationToken cancellationToken)
        {
            try
            {
                var _maintenanceRequest = await _dbContext.MaintenanceRequests.FirstOrDefaultAsync(o => o.Id == request.dto.Id, cancellationToken);

                if (_maintenanceRequest == null)
                {
                    return new UpdateMaintenanceRequestCommendResult
                    {
                        IsSuccess = false,
                        Errors = new List<string> { "Maintenance Request not found" },
                        ErrorCode = Domain.Common.ErrorCode.NotFound
                    };
                }

                // Map the DTO to the existing entity (_maintenanceRequest)
                _maintenanceRequest.RequestNumber = request.dto.RequestNumber;
                _maintenanceRequest.RequestDate = request.dto.RequestDate;
                _maintenanceRequest.DeliveryDate = request.dto.DeliveryDate;
                _maintenanceRequest.CostBearer = request.dto.CostBearer;
                _maintenanceRequest.MaintenanceType = request.dto.MaintenanceType;
                _maintenanceRequest.IsPrivateMaintenance = request.dto.IsPrivateMaintenance;
                _maintenanceRequest.Description = request.dto.Description;
                _maintenanceRequest.MaintenanceRequestFile = request.dto.MaintenanceRequestFile;
                _maintenanceRequest.TenantId = request.dto.TenantId;
                _maintenanceRequest.MemberId = request.dto.MemberId;
                _maintenanceRequest.RealEstateId = request.dto.RealEstateId;
                _maintenanceRequest.RealEstateUnitId = request.dto.RealEstateUnitId;

                // Validate the entity
                var validationResults = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(_maintenanceRequest, new ValidationContext(_maintenanceRequest), validationResults, true);

                if (!isValid)
                {
                    return new UpdateMaintenanceRequestCommendResult
                    {
                        IsSuccess = false,
                        Errors = validationResults.Select(vr => vr.ErrorMessage).ToList(),
                        ErrorCode = Domain.Common.ErrorCode.InvalidDate
                    };
                }

                // Save the updates
                _dbContext.MaintenanceRequests.Update(_maintenanceRequest);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return new UpdateMaintenanceRequestCommendResult
                {
                    IsSuccess = true,
                    Id = _maintenanceRequest.Id
                };
            }
            catch (Exception ex)
            {
                return new UpdateMaintenanceRequestCommendResult
                {
                    IsSuccess = false,
                    Errors = new List<string> { ex.Message },
                    ErrorCode = Domain.Common.ErrorCode.Error
                };
            }
        }
    }
}
