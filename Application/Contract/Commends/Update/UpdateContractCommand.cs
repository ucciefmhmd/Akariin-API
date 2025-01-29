using Application.Contract.Queries.GetAll;
using Application.Utilities.Models;
using Infrastructure;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Contract.Commends.Update
{
    public record UpdateContractCommand(ContractDto dto) : IRequest<UpdateContractCommandResult>;

    public record UpdateContractCommandResult : BaseCommandResult
    {
        public long Id { get; set; }
    }

    public class UpdateContractCommandHandler(ApplicationDbContext _dbContext) : IRequestHandler<UpdateContractCommand, UpdateContractCommandResult>
    {
        public async Task<UpdateContractCommandResult> Handle(UpdateContractCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var contract = await _dbContext.Contracts.FindAsync(new object[] { request.dto.Id }, cancellationToken);

                if (contract == null)
                {
                    return new UpdateContractCommandResult
                    {
                        IsSuccess = false,
                        Id = request.dto.Id,
                        Errors = { "Contract not found." }
                    };
                }

                contract.StartDate = request.dto.StartDate;
                contract.EndDate = request.dto.EndDate;
                contract.DateOfConclusion = request.dto.DateOfConclusion;
                contract.Duration = request.dto.Duration;
                contract.Number = request.dto.Number;
                contract.Type = request.dto.Type;
                contract.TerminationMethod = request.dto.TerminationMethod;
                contract.RealEstateUnitId = request.dto.RealEstateUnitId;
                contract.TenantId = request.dto.TenantId;


                var validationResults = new List<ValidationResult>();

                var isValid = Validator.TryValidateObject(contract, new ValidationContext(contract), validationResults, true);

                if (!isValid)
                {
                    return new UpdateContractCommandResult
                    {
                        IsSuccess = false,
                        Errors = validationResults.Select(vr => vr.ErrorMessage).ToList(),
                        ErrorCode = Domain.Common.ErrorCode.InvalidDate
                    };
                }

                _dbContext.Contracts.Update(contract);

                await _dbContext.SaveChangesAsync(cancellationToken);

                return new UpdateContractCommandResult
                {
                    IsSuccess = true,
                    Id = contract.Id
                };
            }
            catch (Exception ex)
            {
                return new UpdateContractCommandResult
                {
                    IsSuccess = false,
                    Errors = { ex.Message },
                    ErrorCode = Domain.Common.ErrorCode.Error
                };
            }
        }
    }
}
