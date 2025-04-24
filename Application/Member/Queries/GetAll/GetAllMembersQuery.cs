using Application.RealEstateUnit.Queries.GetAll;
using Application.Utilities.Extensions;
using Application.Utilities.Filter;
using Application.Utilities.Models;
using Application.Utilities.Sort;
using Infrastructure;
using MediatR;

namespace Application.Owner.Queries.GetAll
{
    public record GetAllMembersQuery : BasePaginatedQuery, IRequest<GetAllMembersQueryResult> 
    {
        public string? UserId { get; set; }
    }


    public record GetAllMembersQueryResult : BaseCommandResult
    {
        public BasePaginatedList<MemberDto> dto { get; set; }
    }
    public record MemberDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string? City { get; set; }
        public string? Address { get; set; }
        public string PhoneNumber { get; set; }
        public string? Gender { get; set; }
        public string? Nationality { get; set; }
        public string Role { get; set; }
        public CreatedByVM CreatedBy { get; set; }
        public CreatedByVM ModifiedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
    public class GetAllMembersQueryHandler(ApplicationDbContext _dbContext) : IRequestHandler<GetAllMembersQuery, GetAllMembersQueryResult>
    {
        public async Task<GetAllMembersQueryResult> Handle(GetAllMembersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var _owners = await _dbContext.Members
                    .Search(request.SearchTerm)
                    .Where(m => !m.IsDeleted && (m.CreatedById == request.UserId || request.UserId == null))
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
                    })
                    .Filter(request.Filters)
                    .Sort(request.Sorts ?? new List<SortedQuery>() { new SortedQuery() { PropertyName = "Name", Direction = SortDirection.ASC } })
                    .ToPaginatedListAsync(request.PageNumber, request.PageSize);

                return new GetAllMembersQueryResult
                {
                    IsSuccess = true,
                    dto = _owners
                };
            }
            catch (Exception ex)
            {
                return new GetAllMembersQueryResult
                {
                    IsSuccess = false,
                    Errors = { ex.Message },
                    ErrorCode = Domain.Common.ErrorCode.Error
                };
            }
        }
    }
}