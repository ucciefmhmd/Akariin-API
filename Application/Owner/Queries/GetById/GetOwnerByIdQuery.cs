using Application.Owner.Queries.GetAll;
using Application.RealEstateUnit.Queries.GetAll;
using Application.Utilities.Models;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Queries.GetById
{
    public record GetMemberByIdQuery(long id) : IRequest<GetMemberByIdQueryResult>;

    public record GetMemberByIdQueryResult : BaseCommandResult
    {
        public MemberDto? dto { get; set; }
    }

    public class GetMemberByIdQueryHandler(ApplicationDbContext _dbContext) : IRequestHandler<GetMemberByIdQuery, GetMemberByIdQueryResult>
    {
        public async Task<GetMemberByIdQueryResult> Handle(GetMemberByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var _owner = await _dbContext.Members
                    .Where(o => o.Id == request.id)
                    .Select(o => new MemberDto
                    {
                        Id = o.Id,
                        Name = o.Name,
                        City = o.City,
                        Address = o.Address,
                        PhoneNumber = o.PhoneNumber,
                        Gender = o.Gender,
                        Nationality = o.Nationality,
                        Role = o.Role,
                        CreatedBy = o.CreatedBy != null ? new CreatedByVM { Name = o.CreatedBy.Name, Id = o.CreatedBy.Id } : null,
                        ModifiedBy = o.ModifiedBy != null ? new CreatedByVM { Name = o.ModifiedBy.Name, Id = o.ModifiedBy.Id } : null,
                        CreatedDate = o.CreatedDate,
                        ModifiedDate = o.ModifiedDate
                    }).FirstOrDefaultAsync(cancellationToken);

                if(_owner == null)
                {
                    return new GetMemberByIdQueryResult
                    {
                        IsSuccess = false,
                        Errors = { "Member not found." }
                    };
                }

                return new GetMemberByIdQueryResult
                {
                    IsSuccess = true,
                    dto = _owner
                };

            }
            catch (Exception ex)
            {
                return new GetMemberByIdQueryResult
                {
                    IsSuccess = false,
                    Errors = { ex.Message },
                    ErrorCode = Domain.Common.ErrorCode.Error
                };
            }
        }
    } 
}
