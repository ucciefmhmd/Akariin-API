using Application.Owner.Commends.Add;
using Application.Utilities.Models;
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
        public string Amount { get; set; }
        public DateTime Date { get; set; }
        public long Number { get; set; }
        public float Salary { get; set; }
        public float Discount { get; set; }
        public float Tax { get; set; }
        public long ContractId { get; set; }
    }

    public class AddBillCommandHandler(ApplicationDbContext _dbContext) : IRequestHandler<AddBillCommand, AddBillCommandResult>
    {
        public async Task<AddBillCommandResult> Handle(AddBillCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var _bill = new Domain.Models.Bills.Bill
                {
                    Amount = request.dto.Amount,
                    Date = request.dto.Date,
                    Number = request.dto.Number,
                    Salary = request.dto.Salary,
                    Discount = request.dto.Discount,
                    Tax = request.dto.Tax,
                    ContractId = request.dto.ContractId
                };

                var validationResults = new List<ValidationResult>();

                var isValid = Validator.TryValidateObject(_bill, new ValidationContext(_bill), validationResults, true);

                if (!isValid)
                {
                    return new AddBillCommandResult
                    {
                        IsSuccess = false,
                        Errors = validationResults.Select(vr => vr.ErrorMessage).ToList(),
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
                    ErrorCode = Domain.Common.ErrorCode.Error
                };
            }
        }

    }
}
