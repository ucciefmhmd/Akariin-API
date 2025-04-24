using Application.Services.DeleteService;
using Application.Utilities.Models;
using MediatR;

namespace Application.Owner.Commends.Delete
{
    public record DeleteMemberCommand(long Id) : IRequest<DeleteMemberCommandResult>;

    public record DeleteMemberCommandResult : BaseCommandResult
    {
        public long Id { get; set; }
    }
    public class DeleteMemberCommandHandler(ISoftDeleteService _softDeleteService) : IRequestHandler<DeleteMemberCommand, DeleteMemberCommandResult>
    {
        public async Task<DeleteMemberCommandResult> Handle(DeleteMemberCommand request, CancellationToken cancellationToken)
        {
            var (success, message) = await _softDeleteService.SoftDeleteAsync<Domain.Models.Members.Member>(request.Id, cancellationToken);

            return new DeleteMemberCommandResult
            {
                IsSuccess = success,
                Id = request.Id,
                Message = message
            };
        }
    }

}
