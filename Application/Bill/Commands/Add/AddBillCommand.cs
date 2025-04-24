using Application.Utilities.Models;
using Domain.Common;
using Infrastructure;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Bill.Commends.Add
{
    public record AddBillCommand(CreateBillDto dto) : IRequest<AddBillCommandResult>;

    public record AddBillCommandResult : BaseCommandResult
    {
        public long Id { get; set; }
    }
    public record CreateBillDto
    {
        public DateTime BillDate { get; set; }
        public long TenantId { get; set; }
        public string? IssuedBy { get; set; }
        public long MarketerId { get; set; }
        public StatusBills StatusBills { get; set; } = StatusBills.Pending;
        public decimal Salary { get; set; }
        public decimal ConfirmSalary { get; set; }
        public decimal? Discount { get; set; }
        public decimal? Tax { get; set; }
        public decimal TotalAmount { get; set; }
        public long ContractId { get; set; }
    }

    public class AddBillCommandHandler(ApplicationDbContext _dbContext) : IRequestHandler<AddBillCommand, AddBillCommandResult>
    {
        public async Task<AddBillCommandResult> Handle(AddBillCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var tenant = await _dbContext.Members.FindAsync([request.dto.TenantId], cancellationToken: cancellationToken);

                if (tenant == null)
                {
                    return new AddBillCommandResult
                    {
                        IsSuccess = false,
                        Errors = { "Tenant not found." },
                        ErrorCode = ErrorCode.NotFound
                    };
                }

                var marketer = await _dbContext.Members.FindAsync([request.dto.MarketerId], cancellationToken: cancellationToken);

                if (marketer == null)
                {
                    return new AddBillCommandResult
                    {
                        IsSuccess = false,
                        Errors = { "Marketer not found." },
                        ErrorCode = ErrorCode.NotFound
                    };
                }

                var contract = await _dbContext.Contracts.FindAsync([request.dto.ContractId], cancellationToken: cancellationToken);

                if (contract == null)
                {
                    return new AddBillCommandResult
                    {
                        IsSuccess = false,
                        Errors = { "Contract not found." },
                        ErrorCode = ErrorCode.NotFound
                    };
                }

                var _bill = new Domain.Models.Bills.Bill
                {
                    BillDate = request.dto.BillDate,
                    BillNumber = Guid.NewGuid().ToString()[..8],
                    IssuedBy = request.dto.IssuedBy,
                    TotalAmount = request.dto.TotalAmount,
                    Salary = request.dto.Salary,
                    Discount = request.dto.Discount,
                    Tax = request.dto.Tax,
                    ConfirmSalary = request.dto.ConfirmSalary,
                    StatusBills = request.dto.StatusBills,
                    TenantId = request.dto.TenantId,
                    MarketerId = request.dto.MarketerId,
                    ContractId = request.dto.ContractId,
                    IsActive = true,
                    IsDeleted = false,
                };

                var validationResults = new List<ValidationResult>();

                var isValid = Validator.TryValidateObject(_bill, new ValidationContext(_bill), validationResults, true);

                if (!isValid)
                {
                    return new AddBillCommandResult
                    {
                        IsSuccess = false,
                        Errors = [.. validationResults.Select(vr => vr.ErrorMessage)],
                        ErrorCode = Domain.Common.ErrorCode.InvalidDate
                    };
                }

                await _dbContext.Bills.AddAsync(_bill, cancellationToken);

                await _dbContext.SaveChangesAsync(cancellationToken);

                return new AddBillCommandResult
                {
                    IsSuccess = true,
                    Id = _bill.Id
                };
            }
            catch (Exception ex)
            {
                return new AddBillCommandResult
                {
                    IsSuccess = false,
                    Errors = { ex.Message },
                    ErrorCode = ErrorCode.Error
                };
            }
        }

    }
}
