using Application.Services.File;
using Application.Utilities.Models;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
namespace Application.RealEstateUnit.Commends.Add
{
    public record AddRealEstateUnitCommand(string AnnualRent, string Area, string Floor, string UnitNumber, string NumOfRooms, string Type, string? GasMeter, string? WaterMeter, string? ElectricityCalculation, string Status, IFormFile? Image, long? TenantId, long RealEstateId) : IRequest<AddRealEstateUnitCommendResult>;

    public record AddRealEstateUnitCommendResult : BaseCommandResult
    {
        public long Id { get; set; }
    }

    public class AddRealEstateUnitCommendHandler(ApplicationDbContext _dbContext, AttachmentService _attachmentService) : IRequestHandler<AddRealEstateUnitCommand, AddRealEstateUnitCommendResult>
    {
        public async Task<AddRealEstateUnitCommendResult> Handle(AddRealEstateUnitCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var realEstateUnit = new Domain.Models.RealEstateUnits.RealEstateUnit
                {
                    NumOfRooms = request.NumOfRooms,
                    AnnualRent = request.AnnualRent,
                    Area = request.Area,
                    Floor = request.Floor,
                    Type = request.Type,
                    UnitNumber = request.UnitNumber,
                    TenantId = request.TenantId,
                    WaterMeter = request.WaterMeter,
                    Status = request.Status,
                    RealEstateId = request.RealEstateId,
                    GasMeter = request.GasMeter,
                    ElectricityCalculation = request.ElectricityCalculation,
                    IsActive = true,
                    IsDeleted = false,
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
                if (request.Image is not null)
                {
                    await _attachmentService.UploadFilesAsync(Path.Combine("profiles", realEstateUnit.Id.ToString()), request.Image);
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

