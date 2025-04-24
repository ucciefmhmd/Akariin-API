using Application.Services.DeleteService;
using Application.Utilities.Models;
using MediatR;

namespace Application.Owner.Commends.Delete
{
    public record DeleteMaintenanceRequestCommend(long Id) : IRequest<DeleteMaintenanceRequestCommendResult>;

    public record DeleteMaintenanceRequestCommendResult : BaseCommandResult
    {
        public long Id { get; set; }
    }
    public class DeleteMaintenanceRequestCommendHandler(ISoftDeleteService _softDeleteService) : IRequestHandler<DeleteMaintenanceRequestCommend, DeleteMaintenanceRequestCommendResult>
    {
        public async Task<DeleteMaintenanceRequestCommendResult> Handle(DeleteMaintenanceRequestCommend request, CancellationToken cancellationToken)
        {
            var (success, message) = await _softDeleteService.SoftDeleteAsync<Domain.Models.MaintenanceRequests.MaintenanceRequest>(request.Id, cancellationToken);

            return new DeleteMaintenanceRequestCommendResult
            {
                IsSuccess = success,
                Id = request.Id,
                Message = message
            };
        }
    }

}