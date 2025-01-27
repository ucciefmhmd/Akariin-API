using Application.Utilities.Models;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Bill.Queries.GetAll
{
    public record GetAllBillQuery : IRequest<GetAllBillQueryResult>;

    public record GetAllBillQueryResult : BaseCommandResult
    {
        public List<BillDto> BillDtos { get; set; }
    }

    public record BillDto
    {
        public long Id { get; set; }
        public string Amount { get; set; }
        public DateOnly Date { get; set; }
        public long Number { get; set; }
        public float Salary { get; set; }
        public float Discount { get; set; }
        public float Tax { get; set; }
        public long ContractId { get; set; }
    }

    public class GetAllBillQueryHandler(ApplicationDbContext _dbContext): IRequestHandler<GetAllBillQuery, GetAllBillQueryResult>
    {
        public async Task<GetAllBillQueryResult> Handle(GetAllBillQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var bills = await _dbContext.Bills
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
                                            }).ToListAsync(cancellationToken);

                return new GetAllBillQueryResult
                {
                    IsSuccess = true,
                    BillDtos = bills
                };

            }
            catch (Exception ex)
            {
                return new GetAllBillQueryResult
                {
                    IsSuccess = false,
                    Errors = { ex.Message }
                };
            }
        }

    }
}
