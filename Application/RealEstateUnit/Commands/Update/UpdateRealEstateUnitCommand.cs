using Application.Utilities.Models;
using MediatR;
using System.ComponentModel.DataAnnotations;
using Infrastructure;
using Application.Services.File;
using Microsoft.AspNetCore.Http;
using Domain.Common;

namespace Application.RealEstateUnit.Commends.Update
{
    public record UpdateRealEstateUnitCommand(long Id, string AnnualRent, string Area, string Floor, string UnitNumber, string NumOfRooms,
        string Type, string? GasMeter, string? WaterMeter, string? ElectricityCalculation, string Status, IFormFile? Image, 
        long? TenantId, long RealEstateId) : IRequest<UpdateRealEstateUnitCommendResult>;

    public record UpdateRealEstateUnitCommendResult : BaseCommandResult
    {
        public long Id { get; init; }
    }

    public class UpdateRealEstateUnitCommendHandler(ApplicationDbContext _dbContext, AttachmentService _attachmentService) : IRequestHandler<UpdateRealEstateUnitCommand, UpdateRealEstateUnitCommendResult>
    {
        public async Task<UpdateRealEstateUnitCommendResult> Handle(UpdateRealEstateUnitCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var realEstateUnit = await _dbContext.RealEstateUnits.FindAsync([request.Id], cancellationToken);

                if (realEstateUnit == null)
                {
                    return new UpdateRealEstateUnitCommendResult
                    {
                        IsSuccess = false,
                        Id = request.Id,
                        Errors = { "Real estate unit not found." }
                    };
                }

                realEstateUnit.Id = request.Id;
                realEstateUnit.AnnualRent = request.AnnualRent;
                realEstateUnit.Area = request.Area;
                realEstateUnit.Floor = request.Floor;
                realEstateUnit.NumOfRooms = request.NumOfRooms;
                realEstateUnit.Type = request.Type;
                realEstateUnit.UnitNumber = request.UnitNumber;
                realEstateUnit.TenantId = request.TenantId;
                realEstateUnit.WaterMeter = request.WaterMeter;
                realEstateUnit.RealEstateId = request.RealEstateId;
                realEstateUnit.GasMeter = request.GasMeter;
                realEstateUnit.ElectricityCalculation = request.ElectricityCalculation;
                realEstateUnit.Status = request.Status;

                if (request.Image is not null)
                {
                    if (!string.IsNullOrEmpty(realEstateUnit.Image))
                        await _attachmentService.DeleteFilesAsync(realEstateUnit.Id.ToString());

                    await _attachmentService.UploadFilesAsync(Path.Combine("profiles", request.Id.ToString()), request.Image);
                }

                var validationResults = new List<ValidationResult>();

                var isValid = Validator.TryValidateObject(realEstateUnit, new ValidationContext(realEstateUnit), validationResults, true);

                if (!isValid)
                {
                    return new UpdateRealEstateUnitCommendResult
                    {
                        IsSuccess = false,
                        Errors = [.. validationResults.Select(vr => vr.ErrorMessage)],
                        ErrorCode = ErrorCode.InvalidDate
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
                    ErrorCode = ErrorCode.Error
                };
            }
        }
    }

}
