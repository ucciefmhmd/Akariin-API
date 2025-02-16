using Application.Bill.Queries.GetAll;
using Application.Utilities.Models;
using Infrastructure;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Bill.Commends.Update
{
    public record UpdateBillCommand(BillDto dto) : IRequest<UpdateBillCommandResult>;

    public record UpdateBillCommandResult : BaseCommandResult
    {
        public long Id { get; set; }
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

                bill.Amount = request.dto.Amount;
                bill.Date = request.dto.Date;
                bill.Number = request.dto.Number;
                bill.Salary = request.dto.Salary;
                bill.Discount = request.dto.Discount;
                bill.Tax = request.dto.Tax;
                bill.ContractId = request.dto.ContractId;

                var validationResults = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(bill, new ValidationContext(bill), validationResults, true);

                if (!isValid)
                {
                    return new UpdateBillCommandResult
                    {
                        IsSuccess = false,
                        Errors = validationResults.Select(vr => vr.ErrorMessage).ToList(),
                        ErrorCode = Domain.Common.ErrorCode.InvalidDate
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
