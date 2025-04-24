using Application.Bill.Commends.Add;
using Application.Utilities.Models;
using Domain.Common;
using Infrastructure;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Bill.Commends.Update
{
    public record UpdateBillCommand(UpdateBillDto dto) : IRequest<UpdateBillCommandResult>;

    public record UpdateBillCommandResult : BaseCommandResult
    {
        public long Id { get; set; }
    }

    public record UpdateBillDto
    {
        public long Id { get; set; }
        public DateTime BillDate { get; set; }
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
    }

    public class UpdateBillCommandHandler(ApplicationDbContext _dbContext) : IRequestHandler<UpdateBillCommand, UpdateBillCommandResult>
    {
        public async Task<UpdateBillCommandResult> Handle(UpdateBillCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var bill = await _dbContext.Bills.FindAsync(request.dto.Id);

                if (bill == null)
                {
                    return new UpdateBillCommandResult
                    {
                        IsSuccess = false,
                        Errors = { "Bill not found." }
                    };
                }

                var tenant = await _dbContext.Members.FindAsync([request.dto.TenantId], cancellationToken: cancellationToken);

                if (tenant == null)
                {
                    return new UpdateBillCommandResult
                    {
                        IsSuccess = false,
                        Errors = { "Tenant not found." },
                        ErrorCode = ErrorCode.NotFound
                    };
                }

                var marketer = await _dbContext.Members.FindAsync([request.dto.MarketerId], cancellationToken: cancellationToken);

                if (marketer == null)
                {
                    return new UpdateBillCommandResult
                    {
                        IsSuccess = false,
                        Errors = { "Marketer not found." },
                        ErrorCode = ErrorCode.NotFound
                    };
                }

                var contract = await _dbContext.Contracts.FindAsync([request.dto.ContractId], cancellationToken: cancellationToken);

                if (contract == null)
                {
                    return new UpdateBillCommandResult
                    {
                        IsSuccess = false,
                        Errors = { "Contract not found." },
                        ErrorCode = ErrorCode.NotFound
                    };
                }

                bill.BillDate = request.dto.BillDate;
                bill.IssuedBy = request.dto.IssuedBy;
                bill.TotalAmount = request.dto.TotalAmount;
                bill.Salary = request.dto.Salary;
                bill.ConfirmSalary = request.dto.ConfirmSalary;
                bill.StatusBills = request.dto.StatusBills;
                bill.Discount = request.dto.Discount;
                bill.Tax = request.dto.Tax;
                bill.TenantId = request.dto.TenantId;
                bill.MarketerId = request.dto.MarketerId;
                bill.ContractId = request.dto.ContractId;

                var validationResults = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(bill, new ValidationContext(bill), validationResults, true);

                if (!isValid)
                {
                    return new UpdateBillCommandResult
                    {
                        IsSuccess = false,
                        Errors = [.. validationResults.Select(vr => vr.ErrorMessage)],
                        ErrorCode = ErrorCode.InvalidDate
                    };
                }

                _dbContext.Bills.Update(bill);

                await _dbContext.SaveChangesAsync(cancellationToken);

                return new UpdateBillCommandResult
                {
                    IsSuccess = true,
                    Id = bill.Id
                };

            }
            catch (Exception ex)
            {
                return new UpdateBillCommandResult
                {
                    IsSuccess = false,
                    Errors = { ex.Message }
                };
            }

        }
    }
}
