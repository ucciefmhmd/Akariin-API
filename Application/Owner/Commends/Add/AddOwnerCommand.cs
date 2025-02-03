using Application.Utilities.Models;
using Infrastructure;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Owner.Commends.Add
{
    public record AddOwnerCommand(CreateOnwerDto createOnwerDto) : IRequest<AddOwnerCommandResult>;
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
        public DateOnly? Birthday { get; set; }
        public string? Nationality { get; set; }
        public string Role { get; set; }
        public string? IdNumber { get; set; }
    }

    public class AddOwnerCommandResultHandler(ApplicationDbContext _dbContext) : IRequestHandler<AddOwnerCommand, AddOwnerCommandResult>
    {
        public async Task<AddOwnerCommandResult> Handle(AddOwnerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var _owner = new Domain.Models.Owners.Owner
                {
                    City = request.createOnwerDto.City,
                    Address = request.createOnwerDto.Address,
                    PhoneNumber = request.createOnwerDto.PhoneNumber,
                    Nationality = request.createOnwerDto.Nationality,
                    Role = request.createOnwerDto.Role,
                    Name = request.createOnwerDto.Name,
                    Birthday = request.createOnwerDto.Birthday,
                    Gender = request.createOnwerDto.Gender
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