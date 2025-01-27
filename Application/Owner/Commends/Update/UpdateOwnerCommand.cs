using Application.Utilities.Models;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.Common.Enums.OwnerEnum;

namespace Application.Owner.Commends.Update
{
    public record UpdateOwnerCommand(UpdateOwnerDto UpdateOwnerDto) : IRequest<UpdateOwnerCommandResult>;

    public record UpdateOwnerCommandResult : BaseCommandResult
    {
        public long Id { get; set; }
    }

    public record UpdateOwnerDto
    {
        public long Id { get; set; }
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
    public class UpdateOwnerCommandHandler(ApplicationDbContext _dbContext) : IRequestHandler<UpdateOwnerCommand, UpdateOwnerCommandResult>
    {
        public async Task<UpdateOwnerCommandResult> Handle(UpdateOwnerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var owner = await _dbContext.Owners.FirstOrDefaultAsync(o => o.Id == request.UpdateOwnerDto.Id, cancellationToken);

                if (owner == null)
                {
                    return new UpdateOwnerCommandResult
                    {
                        IsSuccess = false,
                        Errors = new List<string> { "Owner not found" },
                        ErrorCode = Domain.Common.ErrorCode.NotFound
                    };
                }

                owner.Email = request.UpdateOwnerDto.Email;
                owner.City = request.UpdateOwnerDto.City;
                owner.Address = request.UpdateOwnerDto.Address;
                owner.PhoneNumber = request.UpdateOwnerDto.PhoneNumber;
                owner.Nationality = request.UpdateOwnerDto.Nationality;
                owner.Role = request.UpdateOwnerDto.Role;
                owner.IdNumber = request.UpdateOwnerDto.IdNumber;
                owner.Name = request.UpdateOwnerDto.Name;
                owner.Birthday = request.UpdateOwnerDto.Birthday;
                owner.Gender = request.UpdateOwnerDto.Gender;

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
