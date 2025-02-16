using Application.RealEstate.Commends.UpdateRealEstate;
using Application.Services.File;
using Application.Utilities.Models;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
namespace Application.RealEstate.Commends.AddRealEstate
{
    public record AddRealEstateCommand(string Name, string Type, string Category, string Service, string? DocumentType, string? DocumentName, string? DocumentNumber, DateTime? IssueDate, string? Guard, long? GuardId, string? GuardMobile, string? AdNumber, string? ElectricityCalculation, string? GasMeter, string? WaterMeter, IFormFile? Image, string Status, long OwnerId) : IRequest<AddRealEstateCommandResult>;

    public record AddRealEstateCommandResult : BaseCommandResult
    {
        public long Id { get; set; }
    }

    public class AddRealEstateCommandHandler(ApplicationDbContext _dbContext, AttachmentService _attachmentService, IHttpContextAccessor _httpContextAccessor) : IRequestHandler<AddRealEstateCommand, AddRealEstateCommandResult>
    {
        public async Task<AddRealEstateCommandResult> Handle(AddRealEstateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var realEstate = new Domain.Models.RealEstates.RealEstate
                {
                    Name = request.Name,
                    Type = request.Type,
                    Category = request.Category,
                    Service = request.Service,
                    DocumentType = request.DocumentType,
                    DocumentName = request.DocumentName,
                    DocumentNumber = request.DocumentNumber,
                    IssueDate = request.IssueDate,
                    Guard = request.Guard,
                    GuardId = request.GuardId,
                    GuardMobile = request.GuardMobile,
                    AdNumber = request.AdNumber,
                    ElectricityCalculation = request.ElectricityCalculation,
                    GasMeter = request.GasMeter,
                    WaterMeter = request.WaterMeter,
                    Status = request.Status,
                    OwnerId = request.OwnerId
                };

                var validationResults = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(realEstate, new ValidationContext(realEstate), validationResults, true);

                if (!isValid)
                {
                    return new AddRealEstateCommandResult
                    {
                        IsSuccess = false,
                        Errors = validationResults.Select(vr => vr.ErrorMessage).ToList(),
                        ErrorCode = Domain.Common.ErrorCode.InvalidDate
                    };
                }

                await _dbContext.RealEstates.AddAsync(realEstate, cancellationToken);

                await _dbContext.SaveChangesAsync(cancellationToken);

                // Upload the image if provided
                if (request.Image is not null)
                {
                    // Upload new image
                    await _attachmentService.UploadFilesAsync(Path.Combine("profiles", realEstate.Id.ToString()), request.Image);
                }

                return new AddRealEstateCommandResult
                {
                    IsSuccess = true,
                    Id = realEstate.Id
                };
            }
            catch (Exception ex)
            {
                return new AddRealEstateCommandResult
                {
                    IsSuccess = false,
                    Errors = { ex.Message },
                    ErrorCode = Domain.Common.ErrorCode.Error
                };
            }
        }
    }

}
