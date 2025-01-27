using Application.Utilities.Models;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Commends.Delete
{
    public record DeleteOwnerCommand(long Id) : IRequest<DeleteOwnerCommandResult>;

    public record DeleteOwnerCommandResult : BaseCommandResult
    {
    }
    public class DeleteOwnerCommandHandler(ApplicationDbContext _dbContext) : IRequestHandler<DeleteOwnerCommand, DeleteOwnerCommandResult>
    {
        public async Task<DeleteOwnerCommandResult> Handle(DeleteOwnerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var owner = await _dbContext.Owners
                    .FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken);

                if (owner == null)
                {
                    return new DeleteOwnerCommandResult
                    {
                        IsSuccess = false,
                        Errors = new List<string> { "Owner not found" },
                        ErrorCode = Domain.Common.ErrorCode.NotFound
                    };
                }

                _dbContext.Owners.Remove(owner);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return new DeleteOwnerCommandResult
                {
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new DeleteOwnerCommandResult
                {
                    IsSuccess = false,
                    Errors = { ex.Message },
                    ErrorCode = Domain.Common.ErrorCode.Error
                };
            }
        }
    }

}
