using Application.Utilities.Models;
using MediatR;
using System.ComponentModel.DataAnnotations;
using Infrastructure;
using Application.Services.File;
using Microsoft.AspNetCore.Http;

namespace Application.RealEstateUnit.Commends.Update
{
    public record UpdateRealEstateUnitCommand(UpdateRealEstateUnitDto dto) : IRequest<UpdateRealEstateUnitCommendResult>;

    public record UpdateRealEstateUnitCommendResult : BaseCommandResult
    {
        public long Id { get; init; }
    }

    public record UpdateRealEstateUnitDto
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
        public IFormFile? Image { get; set; }
        public long TenantId { get; set; }
        public long RealEstateId { get; set; }

    }

    public class UpdateRealEstateUnitCommendHandler(ApplicationDbContext _dbContext, AttachmentService _attachmentService) : IRequestHandler<UpdateRealEstateUnitCommand, UpdateRealEstateUnitCommendResult>
    {
        public async Task<UpdateRealEstateUnitCommendResult> Handle(UpdateRealEstateUnitCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var realEstateUnit = await _dbContext.RealEstateUnits.FindAsync(new object[] { request.dto.Id }, cancellationToken);

                if (realEstateUnit == null)
                {
                    return new UpdateRealEstateUnitCommendResult
                    {
                        IsSuccess = false,
                        Id = request.dto.Id,
                        Errors = { "Real estate unit not found." }
                    };
                }

                realEstateUnit.Id = request.dto.Id;
                realEstateUnit.AnnualRent = request.dto.AnnualRent;
                realEstateUnit.Area = request.dto.Area;
                realEstateUnit.Floor = request.dto.Floor;
                realEstateUnit.NumOfRooms = request.dto.NumOfRooms;
                realEstateUnit.Type = request.dto.Type;
                realEstateUnit.UnitNumber = request.dto.UnitNumber;
                realEstateUnit.TenantId = request.dto.TenantId;
                realEstateUnit.WaterMeter = request.dto.WaterMeter;
                realEstateUnit.RealEstateId = request.dto.RealEstateId;
                realEstateUnit.GasMeter = request.dto.GasMeter;
                realEstateUnit.ElectricityCalculation = request.dto.ElectricityCalculation;
                if (request.dto.Image is not null)
                {
                    await _attachmentService.UploadFilesAsync(Path.Combine("profiles", request.dto.Id.ToString()), request.dto.Image);
                }

                var validationResults = new List<ValidationResult>();

                var isValid = Validator.TryValidateObject(realEstateUnit, new ValidationContext(realEstateUnit), validationResults, true);

                if (!isValid)
                {
                    return new UpdateRealEstateUnitCommendResult
                    {
                        IsSuccess = false,
                        Errors = validationResults.Select(vr => vr.ErrorMessage).ToList(),
                        ErrorCode = Domain.Common.ErrorCode.InvalidDate
                    };
                }

                _dbContext.RealEstateUnits.Update(realEstateUnit);

                await _dbContext.SaveChangesAsync(cancellationToken);

                return new UpdateRealEstateUnitCommendResult
                {
                    IsSuccess = true,
                    Id = realEstateUnit.Id
                };
            }
            catch (Exception ex)
            {
                return new UpdateRealEstateUnitCommendResult
                {
                    IsSuccess = false,
                    Errors = { ex.Message },
                    ErrorCode = Domain.Common.ErrorCode.Error
                };
            }
        }
    }

}
