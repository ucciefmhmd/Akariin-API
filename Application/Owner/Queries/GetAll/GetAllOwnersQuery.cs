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
        public DateTime ModifiedDate { get; set; }
    }
    public class GetAllMembersQueryHandler(ApplicationDbContext _dbContext) : IRequestHandler<GetAllMembersQuery, GetAllMembersQueryResult>
    {
        public async Task<GetAllMembersQueryResult> Handle(GetAllMembersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var _owners = await _dbContext.Members
                    .Search(request.SearchTerm)
                    .Where(i => i.CreatedById == request.UserId || request.UserId == null)
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