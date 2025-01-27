
using Application.Utilities.Models;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Domain.Common.Enums.OwnerEnum;

namespace Application.Owner.Queries.GetAll
{
    public record GetAllOwnersQuery : IRequest<GetAllOwnersQueryResult>;

    public record GetAllOwnersQueryResult : BaseCommandResult
    {
        public List<OwnerDto> OwnerDtos { get; set; }
    }
    public record OwnerDto
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
    public class GetAllOwnersQueryHandler(ApplicationDbContext _dbContext) : IRequestHandler<GetAllOwnersQuery, GetAllOwnersQueryResult>
    {
        public async Task<GetAllOwnersQueryResult> Handle(GetAllOwnersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var _owners = await _dbContext.Owners
                    .Select(o => new OwnerDto
                    {
                        Id = o.Id,
                        Name = o.Name,
                        Email = o.Email,
                        City = o.City,
                        Address = o.Address,
                        PhoneNumber = o.PhoneNumber,
                        Gender = o.Gender,
                        Birthday = o.Birthday,
                        Nationality = o.Nationality,
                        Role = o.Role,
                        IdNumber = o.IdNumber
                    })
                    .ToListAsync(cancellationToken);

                return new GetAllOwnersQueryResult
                {
                    IsSuccess = true,
                    OwnerDtos = _owners
                };
            }
            catch (Exception ex)
            {
                return new GetAllOwnersQueryResult
                {
                    IsSuccess = false,
                    Errors = { ex.Message },
                    ErrorCode = Domain.Common.ErrorCode.Error
                };
            }
        }
    }
}