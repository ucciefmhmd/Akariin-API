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

    public class DeleteBillCommandHandler(ApplicationDbContext _dbContext) : IRequestHandler<DeleteBillCommand, DeleteBillCommandResult>
    {
        public async Task<DeleteBillCommandResult> Handle(DeleteBillCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var bill = await _dbContext.Bills.FindAsync(request.Id);
                
                if (bill == null)
                {
                    return new DeleteBillCommandResult
                    {
                        IsSuccess = false,
                        Errors = { "Bill not found." }
                    };
                }

                _dbContext.Bills.Remove(bill);

                await _dbContext.SaveChangesAsync(cancellationToken);

                return new DeleteBillCommandResult
                {
                    IsSuccess = true,
                    Id = bill.Id
                };

            }
            catch (Exception ex)
            {
                return new DeleteBillCommandResult
                {
                    IsSuccess = false,
                    Errors = { ex.Message }
                };
            }
        }
    }

}
