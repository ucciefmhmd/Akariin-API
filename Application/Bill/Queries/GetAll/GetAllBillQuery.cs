using Application.RealEstateUnit.Queries.GetAll;
using Application.Utilities.Extensions;
using Application.Utilities.Filter;
using Application.Utilities.Models;
using Application.Utilities.Sort;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Bill.Queries.GetAll
{
    public record GetAllBillQuery : BasePaginatedQuery, IRequest<GetAllBillQueryResult>
    {
        public string? UserId { get; init; }
    }
    public record GetAllBillQueryResult : BaseCommandResult
    {
        public BasePaginatedList<BillDto> dto { get; set; }
    }

    public record BillDto
    {
        public long Id { get; set; }
        public string Amount { get; set; }
        public DateTime Date { get; set; }
        public long Number { get; set; }
        public float Salary { get; set; }
        public float Discount { get; set; }
        public float Tax { get; set; }
        public long ContractId { get; set; }
        public CreatedByVM CreatedBy { get; set; }
        public CreatedByVM ModifiedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }

    public class GetAllBillQueryHandler(ApplicationDbContext _dbContext): IRequestHandler<GetAllBillQuery, GetAllBillQueryResult>
    {
        public async Task<GetAllBillQueryResult> Handle(GetAllBillQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var bills = await _dbContext.Bills
                                            .Include(b => b.Contract)
                                            .Search(request.SearchTerm)
                                            .Where(b => b.CreatedById == request.UserId || request.UserId == null)
                                            .Select(b => new BillDto
                                            {
                                                Id = b.Id,
                                                Amount = b.Amount,
                                                Date = b.Date,
                                                Number = b.Number,
                                                Salary = b.Salary,
                                                Discount = b.Discount,
                                                Tax = b.Tax,
                                                ContractId = b.Contract.Id,
                                                CreatedBy = b.CreatedBy != null ? new CreatedByVM { Name = b.CreatedBy.Name, Id = b.CreatedBy.Id } : null,
                                                ModifiedBy = b.ModifiedBy != null ? new CreatedByVM { Name = b.ModifiedBy.Name, Id = b.ModifiedBy.Id } : null,
                                                CreatedDate = b.CreatedDate,
                                                ModifiedDate = b.ModifiedDate
                                            })
                                            .Filter(request.Filters)
                                            .Sort(request.Sorts ?? new List<SortedQuery>() { new SortedQuery() { PropertyName = "Number", Direction = SortDirection.ASC } })
                                            .ToPaginatedListAsync(request.PageNumber, request.PageSize);

                return new GetAllBillQueryResult
                {
                    IsSuccess = true,
                    dto = bills
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
