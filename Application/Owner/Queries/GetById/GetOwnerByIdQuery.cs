using Application.Owner.Queries.GetAll;
using Application.Utilities.Models;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Queries.GetById
{
    public record GetOwnerByIdQuery(long id) : IRequest<GetOwnerByIdQueryResult>;

    public record GetOwnerByIdQueryResult : BaseCommandResult
    {
        public OwnerDto? OwnerDtos { get; set; }
    }

    public class GetOwnerByIdQueryHandler(ApplicationDbContext _dbContext) : IRequestHandler<GetOwnerByIdQuery, GetOwnerByIdQueryResult>
    {
        public async Task<GetOwnerByIdQueryResult> Handle(GetOwnerByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var _owner = await _dbContext.Owners
                    .Where(o => o.Id == request.id)
                    .Select(o => new OwnerDto
                    {
                        Id = o.Id,
                        Name = o.Name,
                        City = o.City,
                        Address = o.Address,
                        PhoneNumber = o.PhoneNumber,
                        Birthday = o.Birthday,
                        Gender = o.Gender,
                        Nationality = o.Nationality
                    }).FirstOrDefaultAsync(cancellationToken);

                if(_owner == null)
                {
                    return new GetOwnerByIdQueryResult
                    {
                        IsSuccess = false,
                        Errors = { "Owner not found." }
                    };
                }

                return new GetOwnerByIdQueryResult
                {
                    IsSuccess = true,
                    OwnerDtos = _owner
                };

            }
            catch (Exception ex)
            {
                return new GetOwnerByIdQueryResult
                {
                    IsSuccess = false,
                    Errors = { ex.Message },
                    ErrorCode = Domain.Common.ErrorCode.Error
                };
            }
        }
    } 
}
