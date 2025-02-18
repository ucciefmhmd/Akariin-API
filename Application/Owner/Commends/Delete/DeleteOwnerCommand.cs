using Application.Utilities.Models;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Commends.Delete
{
    public record DeleteMemberCommand(long Id) : IRequest<DeleteMemberCommandResult>;

    public record DeleteMemberCommandResult : BaseCommandResult
    {
    }
    public class DeleteMemberCommandHandler(ApplicationDbContext _dbContext) : IRequestHandler<DeleteMemberCommand, DeleteMemberCommandResult>
    {
        public async Task<DeleteMemberCommandResult> Handle(DeleteMemberCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var member = await _dbContext.Members
                    .FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken);

                if (member == null)
                {
                    return new DeleteMemberCommandResult
                    {
                        IsSuccess = false,
                        Errors = new List<string> { "Owner not found" },
                        ErrorCode = Domain.Common.ErrorCode.NotFound
                    };
                }

                _dbContext.Members.Remove(member);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return new DeleteMemberCommandResult
                {
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new DeleteMemberCommandResult
                {
                    IsSuccess = false,
                    Errors = { ex.Message },
                    ErrorCode = Domain.Common.ErrorCode.Error
                };
            }
        }
    }

}
