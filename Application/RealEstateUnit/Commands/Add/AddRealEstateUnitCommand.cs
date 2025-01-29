using Application.RealEstate.Commends.AddRealEstate;
using Application.Utilities.Models;
using Infrastructure;
using MediatR;
using static Domain.Common.Enums.RealEstateUnitEnum;

namespace Application.RealEstateUnit.Commends.Add
{
    public record AddRealEstateUnitCommand(CreateRealestateUnitDto dto): IRequest<AddRealEstateUnitCommendResult>;

    public record AddRealEstateUnitCommendResult : BaseCommandResult
    {
        public long Id { get; set; }
    }

    public record CreateRealestateUnitDto
    {
        public string AnnualRent { get; set; }
        public string Area { get; set; }
        public string Floor { get; set; }
        public string UnitNumber { get; set; }
        public RealEstateUnitRooms NumOfRooms { get; set; }
        public RealEstateUnitType Type { get; set; }
        public long TenantId { get; set; }
    }

    public class AddRealEstateUnitCommendHandler(ApplicationDbContext _dbContext) : IRequestHandler<AddRealEstateUnitCommand, AddRealEstateUnitCommendResult>
    {
        public async Task<AddRealEstateUnitCommendResult> Handle(AddRealEstateUnitCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var realEstateUnit = new Domain.Models.RealEstateUnits.RealEstateUnit
                {
                    NumOfRooms = request.dto.NumOfRooms,
                    AnnualRent = request.dto.AnnualRent,
                    Area = request.dto.Area,
                    Floor = request.dto.Floor,
                    Type = request.dto.Type,
                    UnitNumber = request.dto.UnitNumber,
                    TenantId = request.dto.TenantId
                };

                await _dbContext.RealEstateUnits.AddAsync(realEstateUnit, cancellationToken);

                await _dbContext.SaveChangesAsync(cancellationToken);

                return new AddRealEstateUnitCommendResult
                {
                    IsSuccess = true,
                    Id = realEstateUnit.Id
                };
            }
            catch (Exception ex)
            {
                return new AddRealEstateUnitCommendResult
                {
                    IsSuccess = false,
                    Errors = { ex.Message },
                    ErrorCode = Domain.Common.ErrorCode.Error
                };
            }
        }
    }


}

