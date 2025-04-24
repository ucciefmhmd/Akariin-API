using Application.Contract.Commends.Delete;
using Application.Services.DeleteService;
using Application.Utilities.Models;
using Infrastructure;
using MediatR;

namespace Application.RealEstate.Commends.DeleteRealEstate
{
    public record DeleteRealEstateCommand(long Id) : IRequest<DeleteRealEstateCommandResult>;

    public record DeleteRealEstateCommandResult : BaseCommandResult
    {
        public long Id { get; set; }
    }

    public class DeleteRealEstateCommandHandler(ISoftDeleteService _softDeleteService) : IRequestHandler<DeleteRealEstateCommand, DeleteRealEstateCommandResult>
    {
        public async Task<DeleteRealEstateCommandResult> Handle(DeleteRealEstateCommand request, CancellationToken cancellationToken)
        {
            var (success, message) = await _softDeleteService.SoftDeleteAsync<Domain.Models.RealEstates.RealEstate>(request.Id, cancellationToken);

            return new DeleteRealEstateCommandResult
            {
                IsSuccess = success,
                Id = request.Id,
                Message = message
            };
        }
    }

}
