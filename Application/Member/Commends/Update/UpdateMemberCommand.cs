using Application.Utilities.Models;
using Domain.Common;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Application.Owner.Commends.Update
{
    public record UpdateMemberCommand(UpdateMemberDto dto) : IRequest<UpdateMemberCommandResult>;

    public record UpdateMemberCommandResult : BaseCommandResult
    {
        public long Id { get; set; }
    }

    public record UpdateMemberDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string? City { get; set; }
        public string? Address { get; set; }
        public string PhoneNumber { get; set; }
        public string IdentityNumber { get; init; }
        public string? UserId { get; set; }
        public string? Gender { get; set; }
        public string? Nationality { get; set; }
        public string Role { get; set; }
    }
    public class UpdateMemberCommandHandler(ApplicationDbContext _dbContext) : IRequestHandler<UpdateMemberCommand, UpdateMemberCommandResult>
    {
        public async Task<UpdateMemberCommandResult> Handle(UpdateMemberCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var owner = await _dbContext.Members.FirstOrDefaultAsync(o => o.Id == request.dto.Id, cancellationToken);

                if (owner == null)
                {
                    return new UpdateMemberCommandResult
                    {
                        IsSuccess = false,
                        Errors = ["Owner not found"],
                        ErrorCode = ErrorCode.NotFound
                    };
                }

                owner.City = request.dto.City;
                owner.Address = request.dto.Address;
                owner.PhoneNumber = request.dto.PhoneNumber;
                owner.Nationality = request.dto.Nationality;
                owner.IdentityNumber = request.dto.IdentityNumber;
                owner.Role = request.dto.Role;
                owner.Name = request.dto.Name;
                owner.Gender = request.dto.Gender;
                owner.ModifiedById = request.dto.UserId;
                owner.ModifiedDate = DateTime.UtcNow;

                var validationResults = new List<ValidationResult>();

                var isValid = Validator.TryValidateObject(owner, new ValidationContext(owner), validationResults, true);

                if (!isValid)
                {
                    return new UpdateMemberCommandResult
                    {
                        IsSuccess = false,
                        Errors = [.. validationResults.Select(vr => vr.ErrorMessage)],
                        ErrorCode = Domain.Common.ErrorCode.InvalidDate
                    };
                }

                _dbContext.Members.Update(owner);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return new UpdateMemberCommandResult
                {
                    IsSuccess = true,
                    Id = owner.Id
                };
            }
            catch (Exception ex)
            {
                return new UpdateMemberCommandResult
                {
                    IsSuccess = false,
                    Errors = [ ex.Message ],
                    ErrorCode = ErrorCode.Error
                };
            }
        }
    }


}
