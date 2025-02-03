using Application.RealEstateUnit.Queries.GetAll;
using Application.Utilities.Models;
using MediatR;
using System.ComponentModel.DataAnnotations;
using Infrastructure;

namespace Application.RealEstateUnit.Commends.Update
{
    public record UpdateRealEstateUnitCommand(RealEstateUnitDto dto) : IRequest<UpdateRealEstateUnitCommendResult>;

    public record UpdateRealEstateUnitCommendResult : BaseCommandResult
    {
        public long Id { get; init; }
    }

    public class UpdateRealEstateUnitCommendHandler(ApplicationDbContext _dbContext) : IRequestHandler<UpdateRealEstateUnitCommand, UpdateRealEstateUnitCommendResult>
    {
        public async Task<UpdateRealEstateUnitCommendResult> Handle(UpdateRealEstateUnitCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var realEstateUnit = await _dbContext.RealEstateUnits.FindAsync(new object[] { request.dto.Id }, cancellationToken);

                if (realEstateUnit == null)
                {
                    return new UpdateRealEstateUnitCommendResult
                    {
                        IsSuccess = false,
                        Id = request.dto.Id,
                        Errors = { "Real estate unit not found." }
                    };
                }

                realEstateUnit.Id = request.dto.Id;
                realEstateUnit.AnnualRent = request.dto.AnnualRent;
                realEstateUnit.Area = request.dto.Area;
                realEstateUnit.Floor = request.dto.Floor;
                realEstateUnit.NumOfRooms = request.dto.NumOfRooms;
                realEstateUnit.Type = request.dto.Type;
                realEstateUnit.UnitNumber = request.dto.UnitNumber;
                realEstateUnit.TenantId = request.dto.TenantId;

                var validationResults = new List<ValidationResult>();

                var isValid = Validator.TryValidateObject(realEstateUnit, new ValidationContext(realEstateUnit), validationResults, true);

                if (!isValid)
                {
                    return new UpdateRealEstateUnitCommendResult
                    {
                        IsSuccess = false,
                        Errors = validationResults.Select(vr => vr.ErrorMessage).ToList(),
                        ErrorCode = Domain.Common.ErrorCode.InvalidDate
                    };
                }

                _dbContext.RealEstateUnits.Update(realEstateUnit);

                await _dbContext.SaveChangesAsync(cancellationToken);

                return new UpdateRealEstateUnitCommendResult
                {
                    IsSuccess = true,
                    Id = realEstateUnit.Id
                };
            }
            catch (Exception ex)
            {
                return new UpdateRealEstateUnitCommendResult
                {
                    IsSuccess = false,
                    Errors = { ex.Message },
                    ErrorCode = Domain.Common.ErrorCode.Error
                };
            }
        }
    }

}
