using Application.Utilities.Models;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Application.Owner.Commends.Update
{
    public record UpdateOwnerCommand(UpdateOwnerDto dto) : IRequest<UpdateOwnerCommandResult>;

    public record UpdateOwnerCommandResult : BaseCommandResult
    {
        public long Id { get; set; }
    }

    public record UpdateOwnerDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public string Nationality { get; set; }
        public string Role { get; set; }
    }
    public class UpdateOwnerCommandHandler(ApplicationDbContext _dbContext) : IRequestHandler<UpdateOwnerCommand, UpdateOwnerCommandResult>
    {
        public async Task<UpdateOwnerCommandResult> Handle(UpdateOwnerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var owner = await _dbContext.Owners.FirstOrDefaultAsync(o => o.Id == request.dto.Id, cancellationToken);

                if (owner == null)
                {
                    return new UpdateOwnerCommandResult
                    {
                        IsSuccess = false,
                        Errors = new List<string> { "Owner not found" },
                        ErrorCode = Domain.Common.ErrorCode.NotFound
                    };
                }

                owner.City = request.dto.City;
                owner.Address = request.dto.Address;
                owner.PhoneNumber = request.dto.PhoneNumber;
                owner.Nationality = request.dto.Nationality;
                owner.Role = request.dto.Role;
                owner.Name = request.dto.Name;
                owner.Gender = request.dto.Gender;

                var validationResults = new List<ValidationResult>();

                var isValid = Validator.TryValidateObject(owner, new ValidationContext(owner), validationResults, true);

                if (!isValid)
                {
                    return new UpdateOwnerCommandResult
                    {
                        IsSuccess = false,
                        Errors = validationResults.Select(vr => vr.ErrorMessage).ToList(),
                        ErrorCode = Domain.Common.ErrorCode.InvalidDate
                    };
                }

                _dbContext.Owners.Update(owner);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return new UpdateOwnerCommandResult
                {
                    IsSuccess = true,
                    Id = owner.Id
                };
            }
            catch (Exception ex)
            {
                return new UpdateOwnerCommandResult
                {
                    IsSuccess = false,
                    Errors = new List<string> { ex.Message },
                    ErrorCode = Domain.Common.ErrorCode.Error
                };
            }
        }
    }


}
