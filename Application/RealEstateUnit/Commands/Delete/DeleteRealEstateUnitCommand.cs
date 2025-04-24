using Application.Services.DeleteService;
using Application.Utilities.Models;
using MediatR;

namespace Application.RealEstateUnit.Commends.Delete
{
    public record DeleteRealEstateUnitCommand(long Id) : IRequest<DeleteRealEstateUnitCommandResult>;

    public record DeleteRealEstateUnitCommandResult : BaseCommandResult
    {
        public long Id { get; set; }
    }

    public class DeleteRealEstateUnitCommandHandler(ISoftDeleteService _softDeleteService) : IRequestHandler<DeleteRealEstateUnitCommand, DeleteRealEstateUnitCommandResult>
    {
        public async Task<DeleteRealEstateUnitCommandResult> Handle(DeleteRealEstateUnitCommand request, CancellationToken cancellationToken)
        {
            var (success, message) = await _softDeleteService.SoftDeleteAsync<Domain.Models.RealEstateUnits.RealEstateUnit>(request.Id, cancellationToken);

            return new DeleteRealEstateUnitCommandResult
            {
                IsSuccess = success,
                Id = request.Id,
                Message = message
            };
        }
    }
}
