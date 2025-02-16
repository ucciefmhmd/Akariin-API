using Application.RealEstateUnit.Queries.GetAll;
using Application.Services.File;
using Application.Utilities.Models;
using Domain.Models.RealEstates;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.RealEstateUnit.Queries.GetById
{
    public record GetRealEstateUnitByIdQuery(long Id) : IRequest<GetRealEstateUnitByIdQueryResult>;

    public record GetRealEstateUnitByIdQueryResult : BaseCommandResult
    {
        public RealEstateUnitDataDto dto { get; set; }
    }
    public record RealEstateUnitDataDto
    {
        public long Id { get; set; }
        public string AnnualRent { get; set; }
        public string Area { get; set; }
        public string Floor { get; set; }
        public string UnitNumber { get; set; }
        public string NumOfRooms { get; set; }
        public string Type { get; set; }
        public string? GasMeter { get; set; }
        public string? WaterMeter { get; set; }
        public string? ElectricityCalculation { get; set; }
        public string? Image { get; set; }
        public string Status { get; set; }
        public long? TenantId { get; set; }
        public long RealEstateId { get; set; }
        public CreatedByVM CreatedBy { get; set; }
        public CreatedByVM ModifiedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

    }

    public class GetRealEstateUnitByIdQueryHandler(ApplicationDbContext _dbContext, AttachmentService _attachmentService, ILogger<GetRealEstateUnitByIdQueryHandler> _logger) : IRequestHandler<GetRealEstateUnitByIdQuery, GetRealEstateUnitByIdQueryResult>
    {
        public async Task<GetRealEstateUnitByIdQueryResult> Handle(GetRealEstateUnitByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var realEstateUnit = await _dbContext.RealEstateUnits
                    .Include(re => re.Tenant)
                    .Where(re => re.Id == request.Id)
                    .Select(re => new RealEstateUnitDataDto
                    {
                        Id = re.Id,
                        AnnualRent = re.AnnualRent,
                        Area = re.Area,
                        Floor = re.Floor,
                        UnitNumber = re.UnitNumber,
                        NumOfRooms = re.NumOfRooms,
                        Type = re.Type,
                        ElectricityCalculation = re.ElectricityCalculation,
                        GasMeter = re.GasMeter,
                        WaterMeter = re.WaterMeter,
                        Status = re.Status,
                        TenantId = re.TenantId,
                        RealEstateId = re.RealEstateId,
                        CreatedBy = re.CreatedBy != null ? new CreatedByVM { Name = re.CreatedBy.Name, Id = re.CreatedBy.Id } : null,
                        ModifiedBy = re.ModifiedBy != null ? new CreatedByVM { Name = re.ModifiedBy.Name, Id = re.ModifiedBy.Id } : null,
                        CreatedDate = re.CreatedDate,
                        ModifiedDate = re.ModifiedDate
                    })
                    .FirstOrDefaultAsync(cancellationToken);

                if (realEstateUnit == null)
                {
                    return new GetRealEstateUnitByIdQueryResult
                    {
                        IsSuccess = false,
                        Errors = { "Real estate unit not found." }
                    };
                }

                var profile = await _attachmentService.GetFilesUrlAsync(Path.Combine("profiles", realEstateUnit.Id.ToString()));

                if (profile?.IsSuccess == true && profile.Urls?.Count > 0)
                {
                    realEstateUnit.Image = profile.Urls[0];
                }

                return new GetRealEstateUnitByIdQueryResult
                {
                    IsSuccess = true,
                    dto = realEstateUnit
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving real estate unit by ID: {Id}", request.Id);
                return new GetRealEstateUnitByIdQueryResult
                {
                    IsSuccess = false,
                    Errors = { ex.Message },
                    ErrorCode = Domain.Common.ErrorCode.Error
                };
            }
        }
    }
}
