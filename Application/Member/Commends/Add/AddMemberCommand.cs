using Application.Utilities.Models;
using IdentityModel;
using Infrastructure;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Owner.Commends.Add
{
    public record AddMemberCommand(CreateMemberDto dto) : IRequest<AddMemberCommandResult>;
    public record AddMemberCommandResult : BaseCommandResult
    {
        public long Id { get; set; }
    }

    public record CreateMemberDto
    {
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

    public class AddMemberCommandResultHandler(ApplicationDbContext _dbContext) : IRequestHandler<AddMemberCommand, AddMemberCommandResult>
    {
        public async Task<AddMemberCommandResult> Handle(AddMemberCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var _member = new Domain.Models.Members.Member
                {
                    City = request.dto.City,
                    Address = request.dto.Address,
                    PhoneNumber = request.dto.PhoneNumber,
                    IdentityNumber = request.dto.IdentityNumber,
                    Nationality = request.dto.Nationality,
                    Role = request.dto.Role,
                    Name = request.dto.Name,
                    Gender = request.dto.Gender,
                    CreatedById = request.dto.UserId,
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true,
                    IsDeleted = false
                };

                var validationResults = new List<ValidationResult>();

                var isValid = Validator.TryValidateObject(_member, new ValidationContext(_member), validationResults, true);

                if (!isValid)
                {
                    return new AddMemberCommandResult
                    {
                        IsSuccess = false,
                        Errors = [.. validationResults.Select(vr => vr.ErrorMessage)],
                        ErrorCode = Domain.Common.ErrorCode.InvalidDate
                    };
                }

                await _dbContext.Members.AddAsync(_member, cancellationToken);

                await _dbContext.SaveChangesAsync(cancellationToken);

                return new AddMemberCommandResult

                {
                    IsSuccess = true,
                    Id = _member.Id
                };
            }
            catch (Exception ex)
            {
                return new AddMemberCommandResult
                {
                    IsSuccess = false,
                    Errors = { ex.Message },
                    ErrorCode = Domain.Common.ErrorCode.Error
                };
            }
        }
    
    }
}