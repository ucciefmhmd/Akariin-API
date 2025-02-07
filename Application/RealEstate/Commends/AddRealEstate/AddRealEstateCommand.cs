using Application.Services.File;
using Application.Utilities.Models;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
namespace Application.RealEstate.Commends.AddRealEstate
{
    public record AddRealEstateCommand(AddReaEstateDto dto) : IRequest<AddRealEstateCommandResult>;

    public record AddRealEstateCommandResult : BaseCommandResult
    {
        public long Id { get; set; }
    }

    public record AddReaEstateDto
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
        public string Service { get; set; }
        public string? DocumentType { get; set; }
        public string? DocumentName { get; set; }
        public string? DocumentNumber { get; set; }
        public DateTime? IssueDate { get; set; }
        public string? Guard { get; set; }
        public long? GuardId { get; set; }
        public string? GuardMobile { get; set; }
        public string? AdNumber { get; set; }
        public string? ElectricityCalculation { get; set; }
        public string? GasMeter { get; set; }
        public string? WaterMeter { get; set; }
        public IFormFile? Image { get; set; }
        public long OwnerId { get; set; }
    }

    public class AddRealEstateCommandHandler(ApplicationDbContext _dbContext, AttachmentService _attachmentService) : IRequestHandler<AddRealEstateCommand, AddRealEstateCommandResult>
    {
        public async Task<AddRealEstateCommandResult> Handle(AddRealEstateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var realEstate = new Domain.Models.RealEstates.RealEstate
                {
                    Name = request.dto.Name,
                    Type = request.dto.Type,
                    Category = request.dto.Category,
                    Service = request.dto.Service,
                    DocumentType = request.dto.DocumentType,
                    DocumentName = request.dto.DocumentName,
                    DocumentNumber = request.dto.DocumentNumber,
                    IssueDate = request.dto.IssueDate,
                    Guard = request.dto.Guard,
                    GuardId = request.dto.GuardId,
                    GuardMobile = request.dto.GuardMobile,
                    AdNumber = request.dto.AdNumber,
                    ElectricityCalculation = request.dto.ElectricityCalculation,
                    GasMeter = request.dto.GasMeter,
                    WaterMeter = request.dto.WaterMeter,
                    OwnerId = request.dto.OwnerId
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
                if (request.dto.Image is not null)
                {
                    await _attachmentService.UploadFilesAsync(Path.Combine("profiles", realEstate.Id.ToString()), request.dto.Image);
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
