using Application.Utilities.Models;
using Infrastructure;
using MediatR;

namespace Application.Contract.Commends.Add
{
    public record AddContractCommand(CreateContractDto dto) : IRequest<AddContractCommandResult>;
    
    public record AddContractCommandResult : BaseCommandResult
    {
        public long Id { get; set; }
    }

    public record CreateContractDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime DateOfConclusion { get; set; }
        public TimeOnly Duration { get; set; }
        public long Number { get; set; }
        public string Type { get; set; }
        public string TerminationMethod { get; set; }
        public long RealEstateUnitId { get; set; }
        public long TenantId { get; set; }
    }

    public class AddContractCommandHandler(ApplicationDbContext _dbContext) : IRequestHandler<AddContractCommand, AddContractCommandResult>
    {
        public async Task<AddContractCommandResult> Handle(AddContractCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var contract = new Domain.Models.Contracts.Contract
                {
                    StartDate = request.dto.StartDate,
                    EndDate = request.dto.EndDate,
                    DateOfConclusion = request.dto.DateOfConclusion,
                    Duration = request.dto.Duration,
                    Number = request.dto.Number,
                    Type = request.dto.Type,
                    TerminationMethod = request.dto.TerminationMethod,
                    RealEstateUnitId = request.dto.RealEstateUnitId,
                    TenantId = request.dto.TenantId
                };

                await _dbContext.Contracts.AddAsync(contract, cancellationToken);

                await _dbContext.SaveChangesAsync(cancellationToken);

                return new AddContractCommandResult
                {
                    IsSuccess = true,
                    Id = contract.Id
                };
            }
            catch (Exception ex)
            {
                return new AddContractCommandResult
                {
                    IsSuccess = false,
                    Errors = { ex.Message },
                    ErrorCode = Domain.Common.ErrorCode.Error
                };

            }
        }
    }
}
