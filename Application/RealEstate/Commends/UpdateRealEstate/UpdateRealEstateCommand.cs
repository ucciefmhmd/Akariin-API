using Application.RealEstate.Queries.GetByIdRealEstate;
using Application.Services.File;
using Application.Utilities.Models;
using Google.Apis.Util;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
namespace Application.RealEstate.Commends.UpdateRealEstate
{
    public record UpdateRealEstateCommand(UpdateRealEstatDataeDto _realEstate) : IRequest<UpdateRealEstateCommandResult>;

    public record UpdateRealEstateCommandResult : BaseCommandResult
    {
        public long Id { get; set; }
    }

    public record UpdateRealEstatDataeDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
        public string Service { get; set; }
        public IFormFile? Image { get; set; }
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
        public long OwnerId { get; set; }
    }

    public class UpdateRealEstateCommandHandler(ApplicationDbContext _dbContext, AttachmentService _attachmentService) : IRequestHandler<UpdateRealEstateCommand, UpdateRealEstateCommandResult>
    {
        public async Task<UpdateRealEstateCommandResult> Handle(UpdateRealEstateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var realEstate = await _dbContext.RealEstates.FindAsync(new object[] { request._realEstate.Id }, cancellationToken);

                if (realEstate == null)
                {
                    return new UpdateRealEstateCommandResult
                    {
                        IsSuccess = false,
                        Id = request._realEstate.Id,
                        Errors = { "Real estate not found." }
                    };
                }

                realEstate.Name = request._realEstate.Name;
                realEstate.Type = request._realEstate.Type;
                realEstate.Category = request._realEstate.Category;
                realEstate.Service = request._realEstate.Service;
                realEstate.DocumentType = request._realEstate.DocumentType;
                realEstate.DocumentName = request._realEstate.DocumentName;
                realEstate.DocumentNumber = request._realEstate.DocumentNumber;
                realEstate.IssueDate = request._realEstate.IssueDate;
                realEstate.Guard = request._realEstate.Guard;
                realEstate.GuardId = request._realEstate.GuardId;
                realEstate.GuardMobile = request._realEstate.GuardMobile;
                realEstate.AdNumber = request._realEstate.AdNumber;
                realEstate.ElectricityCalculation = request._realEstate.ElectricityCalculation;
                realEstate.GasMeter = request._realEstate.GasMeter;
                realEstate.WaterMeter = request._realEstate.WaterMeter;
                realEstate.OwnerId = request._realEstate.OwnerId;
                if (request._realEstate.Image is not null)
                {
                    await _attachmentService.UploadFilesAsync(Path.Combine("profiles", request._realEstate.Id.ToString()), request._realEstate.Image);
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
