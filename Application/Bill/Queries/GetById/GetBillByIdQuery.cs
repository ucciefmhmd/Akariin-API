using Application.Bill.Queries.GetAll;
using Application.RealEstateUnit.Queries.GetAll;
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
        public BillDto dto { get; set; }
    }

    public class GetBillByIdQueryHandler(ApplicationDbContext _dbContext) : IRequestHandler<GetBillByIdQuery, GetBillByIdQueryResult>
    {
        public async Task<GetBillByIdQueryResult> Handle(GetBillByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var bill = await _dbContext.Bills
                    .Include(b => b.Contract)
                    .Where(b => !b.IsDeleted && b.Id == request.Id)
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
                        TenantName = b.Tenant.Name,
                        TenantAddress = b.Tenant.Address,
                        TenantPhoneNumber = b.Tenant.PhoneNumber,
                        MarketerId = b.MarketerId,
                        ContractId = b.Contract.Id,
                        CreatedBy = b.CreatedBy != null ? new CreatedByVM { Name = b.CreatedBy.Name, Id = b.CreatedBy.Id } : null,
                        ModifiedBy = b.ModifiedBy != null ? new CreatedByVM { Name = b.ModifiedBy.Name, Id = b.ModifiedBy.Id } : null,
                        CreatedDate = b.CreatedDate,
                        ModifiedDate = b.ModifiedDate
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
                    dto = bill
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
