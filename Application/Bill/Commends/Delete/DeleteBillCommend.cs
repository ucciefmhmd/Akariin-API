using Application.Utilities.Models;
using Infrastructure;
using MediatR;

namespace Application.Bill.Commends.Delete
{
    public record  DeleteBillCommend(long Id) : IRequest<DeleteBillCommendResult>;

    public record DeleteBillCommendResult : BaseCommandResult
    {
        public long Id { get; set; }
    }

    public class DeleteBillCommendHandler(ApplicationDbContext _dbContext) : IRequestHandler<DeleteBillCommend, DeleteBillCommendResult>
    {
        public async Task<DeleteBillCommendResult> Handle(DeleteBillCommend request, CancellationToken cancellationToken)
        {
            try
            {
                var bill = await _dbContext.Bills.FindAsync(request.Id);
                
                if (bill == null)
                {
                    return new DeleteBillCommendResult
                    {
                        IsSuccess = false,
                        Errors = { "Bill not found." }
                    };
                }

                _dbContext.Bills.Remove(bill);

                await _dbContext.SaveChangesAsync(cancellationToken);

                return new DeleteBillCommendResult
                {
                    IsSuccess = true,
                    Id = bill.Id
                };

            }
            catch (Exception ex)
            {
                return new DeleteBillCommendResult
                {
                    IsSuccess = false,
                    Errors = { ex.Message }
                };
            }
        }
    }

}
