using Application.Utilities.Models;
using Infrastructure;
using MediatR;

namespace Application.RealEstateUnit.Commends.Delete
{
    public record DeleteRealEstateUnitCommand(long Id) : IRequest<DeleteRealEstateUnitCommandResult>;

    public record DeleteRealEstateUnitCommandResult : BaseCommandResult
    {
    }

    public class DeleteRealEstateUnitCommandHandler(ApplicationDbContext _dbContext) : IRequestHandler<DeleteRealEstateUnitCommand, DeleteRealEstateUnitCommandResult>
    {
        public async Task<DeleteRealEstateUnitCommandResult> Handle(DeleteRealEstateUnitCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var realEstateUnit = await _dbContext.RealEstateUnits.FindAsync(new object[] { request.Id }, cancellationToken);

                if (realEstateUnit == null)
                {
                    return new DeleteRealEstateUnitCommandResult
                    {
                        IsSuccess = false,
                        Errors = { "Real estate not found." }
                    };
                }

                _dbContext.RealEstateUnits.Remove(realEstateUnit);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return new DeleteRealEstateUnitCommandResult { IsSuccess = true };
            }
            catch (Exception ex)
            {
                return new DeleteRealEstateUnitCommandResult
                {
                    IsSuccess = false,
                    Errors = { ex.Message },
                    ErrorCode = Domain.Common.ErrorCode.Error
                };
            }
        }
    }
}
