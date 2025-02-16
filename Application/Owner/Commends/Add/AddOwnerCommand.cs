using Application.Utilities.Models;
using Infrastructure;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Owner.Commends.Add
{
    public record AddOwnerCommand(CreateOnwerDto dto) : IRequest<AddOwnerCommandResult>;
    public record AddOwnerCommandResult : BaseCommandResult
    {
        public long Id { get; set; }
    }

    public record CreateOnwerDto
    {
        public string Name { get; set; }
        public string? City { get; set; }
        public string? Address { get; set; }
        public string PhoneNumber { get; set; }
        public string? Gender { get; set; }
        public string? Nationality { get; set; }
        public string Role { get; set; }
    }

    public class AddOwnerCommandResultHandler(ApplicationDbContext _dbContext) : IRequestHandler<AddOwnerCommand, AddOwnerCommandResult>
    {
        public async Task<AddOwnerCommandResult> Handle(AddOwnerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var _owner = new Domain.Models.Owners.Owner
                {
                    City = request.dto.City,
                    Address = request.dto.Address,
                    PhoneNumber = request.dto.PhoneNumber,
                    Nationality = request.dto.Nationality,
                    Role = request.dto.Role,
                    Name = request.dto.Name,
                    Gender = request.dto.Gender
                };

                var validationResults = new List<ValidationResult>();

                var isValid = Validator.TryValidateObject(_owner, new ValidationContext(_owner), validationResults, true);

                if (!isValid)
                {
                    return new AddOwnerCommandResult
                    {
                        IsSuccess = false,
                        Errors = validationResults.Select(vr => vr.ErrorMessage).ToList(),
                        ErrorCode = Domain.Common.ErrorCode.InvalidDate
                    };
                }

                await _dbContext.Owners.AddAsync(_owner, cancellationToken);

                await _dbContext.SaveChangesAsync(cancellationToken);

                return new AddOwnerCommandResult
                {
                    IsSuccess = true,
                    Id = _owner.Id
                };
            }
            catch (Exception ex)
            {
                return new AddOwnerCommandResult
                {
                    IsSuccess = false,
                    Errors = { ex.Message },
                    ErrorCode = Domain.Common.ErrorCode.Error
                };
            }
        }
    
    }
}