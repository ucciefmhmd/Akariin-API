using Application.Services.File;
using Application.Utilities.Models;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
namespace Application.RealEstateUnit.Commends.Add
{
    public record AddRealEstateUnitCommand(CreateRealestateUnitDto dto): IRequest<AddRealEstateUnitCommendResult>;

    public record AddRealEstateUnitCommendResult : BaseCommandResult
    {
        public long Id { get; set; }
    }

    public record CreateRealestateUnitDto
    {
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

    public class AddRealEstateUnitCommendHandler(ApplicationDbContext _dbContext, AttachmentService _attachmentService) : IRequestHandler<AddRealEstateUnitCommand, AddRealEstateUnitCommendResult>
    {
        public async Task<AddRealEstateUnitCommendResult> Handle(AddRealEstateUnitCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var realEstateUnit = new Domain.Models.RealEstateUnits.RealEstateUnit
                {
                    NumOfRooms = request.dto.NumOfRooms,
                    AnnualRent = request.dto.AnnualRent,
                    Area = request.dto.Area,
                    Floor = request.dto.Floor,
                    Type = request.dto.Type,
                    UnitNumber = request.dto.UnitNumber,
                    TenantId = request.dto.TenantId,
                    WaterMeter = request.dto.WaterMeter,
                    RealEstateId = request.dto.RealEstateId,
                    GasMeter = request.dto.GasMeter,
                    ElectricityCalculation = request.dto.ElectricityCalculation,
                };

                var validationResults = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(realEstateUnit, new ValidationContext(realEstateUnit), validationResults, true);

                if (!isValid)
                {
                    return new AddRealEstateUnitCommendResult
                    {
                        IsSuccess = false,
                        Errors = validationResults.Select(vr => vr.ErrorMessage).ToList(),
                        ErrorCode = Domain.Common.ErrorCode.InvalidDate
                    };
                }

                await _dbContext.RealEstateUnits.AddAsync(realEstateUnit, cancellationToken);

                await _dbContext.SaveChangesAsync(cancellationToken);

                // Upload the image if provided
                if (request.dto.Image is not null)
                {
                    await _attachmentService.UploadFilesAsync(Path.Combine("profiles", realEstateUnit.Id.ToString()), request.dto.Image);
                }

                return new AddRealEstateUnitCommendResult
                {
                    IsSuccess = true,
                    Id = realEstateUnit.Id
                };
            }
            catch (Exception ex)
            {
                return new AddRealEstateUnitCommendResult
                {
                    IsSuccess = false,
                    Errors = { ex.Message },
                    ErrorCode = Domain.Common.ErrorCode.Error
                };
            }
        }
    }


}

