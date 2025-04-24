using Application.RealEstateUnit.Queries.GetAll;
using Application.Utilities.Extensions;
using Application.Utilities.Filter;
using Application.Utilities.Models;
using Application.Utilities.Sort;
using Domain.Common;
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
        public DateTime BillDate { get; set; }
        public string BillNumber { get; set; }
        public long TenantId { get; set; }
        public string? IssuedBy { get; set; }
        public long MarketerId { get; set; }
        public StatusBills StatusBills { get; set; } = StatusBills.Pending;
        public decimal Salary { get; set; }
        public decimal? ConfirmSalary { get; set; }
        public decimal? Discount { get; set; }
        public decimal? Tax { get; set; }
        public decimal TotalAmount { get; set; }
        public long ContractId { get; set; }
        public CreatedByVM CreatedBy { get; set; }
        public CreatedByVM ModifiedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
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
                                            .Where(b => !b.IsDeleted && (b.CreatedById == request.UserId || request.UserId == null))
                                            .Select(b => new BillDto
                                            {
                                                Id = b.Id,
                                                BillDate = b.BillDate,
                                                BillNumber = b.BillNumber,
                                                IssuedBy = b.IssuedBy,
                                                TotalAmount = b.TotalAmount,
                                                Salary = b.Salary,
                                                ConfirmSalary = b.ConfirmSalary,
                                                StatusBills = b.StatusBills,
                                                Discount = b.Discount,
                                                Tax = b.Tax,
                                                TenantId = b.TenantId,
                                                MarketerId = b.MarketerId,
                                                ContractId = b.Contract.Id,
                                                CreatedBy = b.CreatedBy != null ? new CreatedByVM { Name = b.CreatedBy.Name, Id = b.CreatedBy.Id } : null,
                                                ModifiedBy = b.ModifiedBy != null ? new CreatedByVM { Name = b.ModifiedBy.Name, Id = b.ModifiedBy.Id } : null,
                                                CreatedDate = b.CreatedDate,
                                                ModifiedDate = b.ModifiedDate
                                            })
                                            .Filter(request.Filters)
                                            .Sort(request.Sorts ?? [new SortedQuery() { PropertyName = "Number", Direction = SortDirection.ASC }])
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
