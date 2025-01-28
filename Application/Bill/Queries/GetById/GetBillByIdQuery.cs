using Application.Bill.Queries.GetAll;
using Application.Utilities.Extensions;
using Application.Utilities.Models;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Bill.Queries.GetById
{
    public record GetBillByIdQuery(long Id) : IRequest<GetBillByIdQueryResult>;

    public record GetBillByIdQueryResult : BaseCommandResult
    {
        public BillDto BillDto { get; set; }
    }

    public class GetBillByIdQueryHandler(ApplicationDbContext _dbContext) : IRequestHandler<GetBillByIdQuery, GetBillByIdQueryResult>
    {
        public async Task<GetBillByIdQueryResult> Handle(GetBillByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var bill = await _dbContext.Bills
                    .Include(b => b.Contract)
                    .Select(b => new BillDto
                    {
                        Id = b.Id,
                        Amount = b.Amount,
                        Date = b.Date,
                        Number = b.Number,
                        Salary = b.Salary,
                        Discount = b.Discount,
                        Tax = b.Tax,
                        ContractId = b.Contract.Id
                    }).FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken);

                if (bill == null)
                {
                    return new GetBillByIdQueryResult
                    {
                        IsSuccess = false,
                        Errors = { "Bill not found" }
                    };
                }

                return new GetBillByIdQueryResult
                {
                    IsSuccess = true,
                    BillDto = bill
                };
            }
            catch (Exception ex)
            {
                return new GetBillByIdQueryResult
                {
                    IsSuccess = false,
                    Errors = { ex.Message }
                };
            }
        }
    }
}
