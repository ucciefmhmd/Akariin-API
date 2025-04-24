using Application.Services.DeleteService;
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

    public class DeleteContractCommandHandler(ISoftDeleteService _softDeleteService) : IRequestHandler<DeleteContractCommand, DeleteContractCommandResult>
    {
        public async Task<DeleteContractCommandResult> Handle(DeleteContractCommand request, CancellationToken cancellationToken)
        {
            var (success, message) = await _softDeleteService.SoftDeleteAsync<Domain.Models.Contracts.Contract>(request.Id, cancellationToken);

            return new DeleteContractCommandResult
            {
                IsSuccess = success,
                Id = request.Id,
                Message = message
            };
        }
    }

}
