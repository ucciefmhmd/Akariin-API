using Application.Utilities.Models;
using Infrastructure;
using MediatR;
using System.ComponentModel.DataAnnotations;
namespace Application.RealEstate.Commends.UpdateRealEstate
{
    public record UpdateRealEstateCommand(long Id, string Name, string Type, string Category, string Service, long OwnerId) : IRequest<UpdateRealEstateCommandResult>;

    public record UpdateRealEstateCommandResult : BaseCommandResult
    {
        public long Id { get; set; }
    }

    public class UpdateRealEstateCommandHandler(ApplicationDbContext _dbContext) : IRequestHandler<UpdateRealEstateCommand, UpdateRealEstateCommandResult>
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
                realEstate.OwnerId = request.OwnerId;

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
