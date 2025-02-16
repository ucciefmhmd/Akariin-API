using Application.RealEstate.Commends.AddRealEstate;
using Application.Services.File;
using Application.Utilities.Models;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
namespace Application.RealEstate.Commends.UpdateRealEstate
{
    public record UpdateRealEstateCommand(long Id, string Name, string Type, string Category, string Service, string? DocumentType, string? DocumentName, string? DocumentNumber, DateTime? IssueDate, string? Guard, long? GuardId, string? GuardMobile, string? AdNumber, string? ElectricityCalculation, string? GasMeter, string? WaterMeter, IFormFile? Image, string Status, long OwnerId) : IRequest<UpdateRealEstateCommandResult>;

    public record UpdateRealEstateCommandResult : BaseCommandResult
    {
        public long Id { get; set; }
    }

    public class UpdateRealEstateCommandHandler(ApplicationDbContext _dbContext, AttachmentService _attachmentService) : IRequestHandler<UpdateRealEstateCommand, UpdateRealEstateCommandResult>
    {
        public async Task<UpdateRealEstateCommandResult> Handle(UpdateRealEstateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var realEstate = await _dbContext.RealEstates.FindAsync(new object[] { request.Id }, cancellationToken);

                if (realEstate == null)
                {
                    return new UpdateRealEstateCommandResult
                    {
                        IsSuccess = false,
                        Id = request.Id,
                        Errors = { "Real estate not found." }
                    };
                }

                realEstate.Name = request.Name;
                realEstate.Type = request.Type;
                realEstate.Category = request.Category;
                realEstate.Service = request.Service;
                realEstate.DocumentType = request.DocumentType;
                realEstate.DocumentName = request.DocumentName;
                realEstate.DocumentNumber = request.DocumentNumber;
                realEstate.IssueDate = request.IssueDate;
                realEstate.Guard = request.Guard;
                realEstate.GuardId = request.GuardId;
                realEstate.GuardMobile = request.GuardMobile;
                realEstate.AdNumber = request.AdNumber;
                realEstate.ElectricityCalculation = request.ElectricityCalculation;
                realEstate.GasMeter = request.GasMeter;
                realEstate.WaterMeter = request.WaterMeter;
                realEstate.Status = request.Status;
                realEstate.OwnerId = request.OwnerId;

                if (request.Image is not null)
                {
                    // Delete old image if it exists
                    if (!string.IsNullOrEmpty(realEstate.Image))
                    {
                        await _attachmentService.DeleteFilesAsync(realEstate.Id.ToString());
                    }

                    // Upload new image
                    await _attachmentService.UploadFilesAsync(Path.Combine("profiles", request.Id.ToString()), request.Image);

                }

                var validationResults = new List<ValidationResult>();

                var isValid = Validator.TryValidateObject(realEstate, new ValidationContext(realEstate), validationResults, true);

                if (!isValid)
                {
                    return new UpdateRealEstateCommandResult
                    {
                        IsSuccess = false,
                        Errors = validationResults.Select(vr => vr.ErrorMessage).ToList(),
                        ErrorCode = Domain.Common.ErrorCode.InvalidDate
                    };
                }

                _dbContext.RealEstates.Update(realEstate);

                await _dbContext.SaveChangesAsync(cancellationToken);

                return new UpdateRealEstateCommandResult 
                {
                    IsSuccess = true,
                    Id = realEstate.Id
                };
            }
            catch (Exception ex)
            {
                return new UpdateRealEstateCommandResult
                {
                    IsSuccess = false,
                    Errors = { ex.Message },
                    ErrorCode = Domain.Common.ErrorCode.Error
                };
            }
        }
    }


}
