using Application.RealEstate.Queries.GetAllRealEstate;
using Application.RealEstateUnit.Queries.GetAll;
using Application.Utilities.Models;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.RealEstateUnit.Queries.GetById
{
    public record GetRealEstateUnitByIdQuery(long Id) : IRequest<GetRealEstateUnitByIdQueryResult>;

    public record GetRealEstateUnitByIdQueryResult : BaseCommandResult
    {
        public RealEstateUnitDto dto { get; set; }
    }
    public class GetRealEstateUnitByIdQueryHandler(ApplicationDbContext _dbContext) : IRequestHandler<GetRealEstateUnitByIdQuery, GetRealEstateUnitByIdQueryResult>
    {
        public async Task<GetRealEstateUnitByIdQueryResult> Handle(GetRealEstateUnitByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var realEstateUnit = await _dbContext.RealEstateUnits
                    .Include(re => re.Tenant)
                    .Where(re => re.Id == request.Id)
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
                    .FirstOrDefaultAsync(cancellationToken);

                if (realEstateUnit == null)
                {
                    return new GetRealEstateUnitByIdQueryResult
                    {
                        IsSuccess = false,
                        Errors = { "Real estate unit not found." }
                    };
                }

                return new GetRealEstateUnitByIdQueryResult
                {
                    IsSuccess = true,
                    dto = realEstateUnit
                };
            }
            catch (Exception ex)
            {
                return new GetRealEstateUnitByIdQueryResult
                {
                    IsSuccess = false,
                    Errors = { ex.Message },
                    ErrorCode = Domain.Common.ErrorCode.Error
                };
            }
        }
    }
}
