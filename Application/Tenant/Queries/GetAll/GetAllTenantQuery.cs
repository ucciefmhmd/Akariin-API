using Application.Utilities.Extensions;
using Application.Utilities.Filter;
using Application.Utilities.Models;
using Application.Utilities.Sort;
using Infrastructure;
using MediatR;

namespace Application.Tenant.Queries.GetAll
{
    public record GetAllTenantQuery : BasePaginatedQuery, IRequest<GetAllTenantQueryResult>;

    public record GetAllTenantQueryResult : BaseCommandResult
    {
        public BasePaginatedList<TenantDto> dto { get; init; }
    }

    public record TenantDto
    {
        public long Id { get; init; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public DateOnly Birthday { get; set; }
        public string Nationality { get; set; }
        public string IdNumber { get; set; }
    }

    public class GetAllTenantQueryHandler(ApplicationDbContext _dbContext) : IRequestHandler<GetAllTenantQuery, GetAllTenantQueryResult> 
    {
        public async Task<GetAllTenantQueryResult> Handle(GetAllTenantQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var tenant = await _dbContext.Tenant
                    .Search(request.SearchTerm)
                    .Select(t => new TenantDto
                    {
                        Id = t.Id,
                        Name = t.Name,
                        Email = t.Email,
                        City = t.City,
                        Address = t.Address,
                        PhoneNumber = t.PhoneNumber,
                        Gender = t.Gender,
                        Birthday = t.Birthday,
                        Nationality = t.Nationality,
                        IdNumber = t.IdNumber
                    })
                    .Filter(request.Filters)
                    .Sort(request.Sorts ?? new List<SortedQuery>() { new SortedQuery() { PropertyName = "Name", Direction = SortDirection.ASC } })
                    .ToPaginatedListAsync(request.PageNumber, request.PageSize);

                return new GetAllTenantQueryResult
                {
                    IsSuccess = true,
                    dto = tenant
                };
            }
            catch (Exception ex)
            {
                return new GetAllTenantQueryResult
                {
                    IsSuccess = false,
                    Errors = { ex.Message },
                    ErrorCode = Domain.Common.ErrorCode.Error
                };
            }
        }

    }


}
