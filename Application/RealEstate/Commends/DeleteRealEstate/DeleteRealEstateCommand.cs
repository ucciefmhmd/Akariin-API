using Application.Utilities.Models;
using Infrastructure;
using MediatR;

namespace Application.RealEstate.Commends.DeleteRealEstate
{
    public record DeleteRealEstateCommand(long Id) : IRequest<DeleteRealEstateCommandResult>;

    public record DeleteRealEstateCommandResult : BaseCommandResult
    {
    }

    public class DeleteRealEstateCommandHandler(ApplicationDbContext _dbContext) : IRequestHandler<DeleteRealEstateCommand, DeleteRealEstateCommandResult>
    {
        public async Task<DeleteRealEstateCommandResult> Handle(DeleteRealEstateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var realEstate = await _dbContext.RealEstates.FindAsync(new object[] { request.Id }, cancellationToken);

                if (realEstate == null)
                {
                    return new DeleteRealEstateCommandResult
                    {
                        IsSuccess = false,
                        Errors = { "Real estate not found." }
                    };
                }

                _dbContext.RealEstates.Remove(realEstate);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return new DeleteRealEstateCommandResult { IsSuccess = true };
            }
            catch (Exception ex)
            {
                return new DeleteRealEstateCommandResult
                {
                    IsSuccess = false,
                    Errors = { ex.Message },
                    ErrorCode = Domain.Common.ErrorCode.Error
                };
            }
        }
    }

}
