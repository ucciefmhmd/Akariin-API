using Application.Utilities.Extensions;
using Application.Utilities.Filter;
using Application.Utilities.Models;
using Application.Utilities.Sort;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.RealEstate.Queries.GetAllRealEstate
{
    public record GetAllRealEstateQuery : BasePaginatedQuery, IRequest<GetAllRealEstateQueryResult>;

    public record GetAllRealEstateQueryResult : BaseCommandResult
    {
        public BasePaginatedList<RealEstateDto> RealEstateDto { get; set; }
    }

    public record RealEstateDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
        public string Service { get; set; }
        public long OwnerId { get; set; }
    }

    public class GetAllRealEstateQueryHandler(ApplicationDbContext _dbContext) : IRequestHandler<GetAllRealEstateQuery, GetAllRealEstateQueryResult>
    {
        public async Task<GetAllRealEstateQueryResult> Handle(GetAllRealEstateQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var realEstates = await _dbContext.RealEstates
                    .Include(re => re.Owner)
                    .Search(request.SearchTerm)
                    .Select(re => new RealEstateDto
                    {
                        Id = re.Id,
                        Name = re.Name,
                        Type = re.Type,
                        Category = re.Category,
                        Service = re.Service,
                        OwnerId = re.Owner.Id
                    })
                    .Filter(request.Filters)
                    .Sort(request.Sorts ?? new List<SortedQuery>() { new SortedQuery() { PropertyName = "Name", Direction = SortDirection.ASC } })
                    .ToPaginatedListAsync(request.PageNumber, request.PageSize);

                return new GetAllRealEstateQueryResult
                {
                    IsSuccess = true,
                    RealEstateDto = realEstates
                };
            }
            catch (Exception ex)
            {
                return new GetAllRealEstateQueryResult
                {
                    IsSuccess = false,
                    Errors = { ex.Message },
                    ErrorCode = Domain.Common.ErrorCode.Error
                };
            }
        }
    }
}