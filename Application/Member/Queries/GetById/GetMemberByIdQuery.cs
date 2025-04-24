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
                    .Where(m => !m.IsDeleted && m.Id == request.id)
                    .Select(m => new MemberDto
                    {
                        Id = m.Id,
                        Name = m.Name,
                        City = m.City,
                        Address = m.Address,
                        PhoneNumber = m.PhoneNumber,
                        Gender = m.Gender,
                        Nationality = m.Nationality,
                        Role = m.Role,
                        CreatedBy = m.CreatedBy != null ? new CreatedByVM { Name = m.CreatedBy.Name, Id = m.CreatedBy.Id } : null,
                        ModifiedBy = m.ModifiedBy != null ? new CreatedByVM { Name = m.ModifiedBy.Name, Id = m.ModifiedBy.Id } : null,
                        CreatedDate = m.CreatedDate,
                        ModifiedDate = m.ModifiedDate
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
