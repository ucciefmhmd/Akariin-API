using Application.Utilities.Extensions;
using Application.Utilities.Filter;
using Application.Utilities.Models;
using Application.Utilities.Sort;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.RealEstateUnit.Queries.GetAll
{
   public record GetAllRealEstateUnitQuery : BasePaginatedQuery, IRequest<GetAllRealEstateUnitQueryResult>;

    public record GetAllRealEstateUnitQueryResult : BaseCommandResult
    {
        public BasePaginatedList<RealEstateUnitDto> dto { get; set; }
    }

    public record RealEstateUnitDto
    {
        public long Id { get; set; }
        public string AnnualRent { get; set; }
        public string Area { get; set; }
        public string Floor { get; set; }
        public string UnitNumber { get; set; }
        public string NumOfRooms { get; set; }
        public string Type { get; set; }
        public long TenantId { get; set; }
    }

    public class GetAllRealEstateUnitQueryHandler(ApplicationDbContext _dbContext) : IRequestHandler<GetAllRealEstateUnitQuery, GetAllRealEstateUnitQueryResult>
    {
        public async Task<GetAllRealEstateUnitQueryResult> Handle(GetAllRealEstateUnitQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var realEstateUnits = await _dbContext.RealEstateUnits
                                                 .Include(re => re.Tenant)
                                                 .Search(request.SearchTerm)
                                                 .Select(re => new RealEstateUnitDto
                                                 {
                                                     Id = re.Id,
                                                     AnnualRent = re.AnnualRent,
                                                     Area = re.Area,
                                                     Floor = re.Floor,
                                                     UnitNumber = re.UnitNumber,
                                                     NumOfRooms = re.NumOfRooms,
                                                     Type = re.Type,
                                                     TenantId = re.TenantId
                                                 })
                                                 .Filter(request.Filters)
                                                 .Sort(request.Sorts ?? new List<SortedQuery>() { new SortedQuery() { PropertyName = "Name", Direction = SortDirection.ASC } })
                                                 .ToPaginatedListAsync(request.PageNumber, request.PageSize);

                return new GetAllRealEstateUnitQueryResult
                {
                    IsSuccess = true,
                    dto = realEstateUnits
                };
            }
            catch (Exception ex)
            {
                return new GetAllRealEstateUnitQueryResult
                {
                    IsSuccess = false,
                    Errors = { ex.Message },
                    ErrorCode = Domain.Common.ErrorCode.Error
                };
            }
        }
    }
}
