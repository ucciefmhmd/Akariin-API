using Application.Utilities.Models;
using Infrastructure;
using MediatR;

namespace Application.Contract.Commends.Delete
{
    public record DeleteContractCommand(long Id) : IRequest<DeleteContractCommandResult>;

    public record DeleteContractCommandResult : BaseCommandResult
    {
        public long Id { get; set; }
    }

    public class DeleteContractCommandHandler(ApplicationDbContext _dbContext) : IRequestHandler<DeleteContractCommand, DeleteContractCommandResult>
    {
        public async Task<DeleteContractCommandResult> Handle(DeleteContractCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var contract = await _dbContext.Contracts.FindAsync(new object[] { request.Id }, cancellationToken);

                if (contract == null)
                {
                    return new DeleteContractCommandResult
                    {
                        IsSuccess = false,
                        Errors = { "Contract not found." }
                    };
                }

                _dbContext.Contracts.Remove(contract);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return new DeleteContractCommandResult { IsSuccess = true };
            }
            catch (Exception ex)
            {
                return new DeleteContractCommandResult
                {
                    IsSuccess = false,
                    Errors = { ex.Message },
                    ErrorCode = Domain.Common.ErrorCode.Error
                };
            }
        }
    }

}
