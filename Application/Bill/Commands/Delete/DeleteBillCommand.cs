using Application.Contract.Commends.Delete;
using Application.Services.DeleteService;
using Application.Utilities.Models;
using Infrastructure;
using MediatR;

namespace Application.Bill.Commends.Delete
{
    public record  DeleteBillCommand(long Id) : IRequest<DeleteBillCommandResult>;

    public record DeleteBillCommandResult : BaseCommandResult
    {
        public long Id { get; set; }
    }

    public class DeleteBillCommandHandler(ISoftDeleteService _softDeleteService) : IRequestHandler<DeleteBillCommand, DeleteBillCommandResult>
    {
        public async Task<DeleteBillCommandResult> Handle(DeleteBillCommand request, CancellationToken cancellationToken)
        {
            var (success, message) = await _softDeleteService.SoftDeleteAsync<Domain.Models.Bills.Bill>(request.Id, cancellationToken);

            return new DeleteBillCommandResult
            {
                IsSuccess = success,
                Id = request.Id,
                Message = message
            };
        }
    }

}
