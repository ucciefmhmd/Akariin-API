using Application.Utilities.Extensions;
using Application.Utilities.Filter;
using Application.Utilities.Models;
using Application.Utilities.Sort;
using Infrastructure;
using MediatR;

namespace Application.Owner.Queries.GetAll
{
    public record GetAllOwnersQuery : BasePaginatedQuery, IRequest<GetAllOwnersQueryResult>;

    public record GetAllOwnersQueryResult : BaseCommandResult
    {
        public BasePaginatedList<OwnerDto> OwnerDtos { get; set; }
    }
    public record OwnerDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string? City { get; set; }
        public string? Address { get; set; }
        public string PhoneNumber { get; set; }
        public string? Gender { get; set; }
        public DateOnly? Birthday { get; set; }
        public string? Nationality { get; set; }
        public string Role { get; set; }
    }
    public class GetAllOwnersQueryHandler(ApplicationDbContext _dbContext) : IRequestHandler<GetAllOwnersQuery, GetAllOwnersQueryResult>
    {
        public async Task<GetAllOwnersQueryResult> Handle(GetAllOwnersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var _owners = await _dbContext.Owners
                    .Search(request.SearchTerm)
                    .Select(o => new OwnerDto
                    {
                        Id = o.Id,
                        Name = o.Name,
                        City = o.City,
                        Address = o.Address,
                        PhoneNumber = o.PhoneNumber,
                        Gender = o.Gender,
                        Birthday = o.Birthday,
                        Nationality = o.Nationality,
                        Role = o.Role
                    })
                    .Filter(request.Filters)
                    .Sort(request.Sorts ?? new List<SortedQuery>() { new SortedQuery() { PropertyName = "Name", Direction = SortDirection.ASC } })
                    .ToPaginatedListAsync(request.PageNumber, request.PageSize);

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