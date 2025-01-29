using Application.Utilities.Models;
using Infrastructure;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.Common.Enums.OwnerEnum;

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
        public string Email { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public Gender Gender { get; set; }
        public DateOnly Birthday { get; set; }
        public string Nationality { get; set; }
        public string Role { get; set; }
        public string IdNumber { get; set; }
    }

    public class AddOwnerCommandResultHandler(ApplicationDbContext _dbContext) : IRequestHandler<AddOwnerCommand, AddOwnerCommandResult>
    {
        public async Task<AddOwnerCommandResult> Handle(AddOwnerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var _owner = new Domain.Models.Owners.Owner
                {
                    Email = request.createOnwerDto.Email,
                    City = request.createOnwerDto.City,
                    Address = request.createOnwerDto.Address,
                    PhoneNumber = request.createOnwerDto.PhoneNumber,
                    Nationality = request.createOnwerDto.Nationality,
                    Role = request.createOnwerDto.Role,
                    IdNumber = request.createOnwerDto.IdNumber,
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