using Application.Utilities.Models;
using Infrastructure;
using MediatR;
using static Domain.Common.Enums.RealEstateEnum;

namespace Application.RealEstate.Commends.AddRealEstate
{
    public record AddRealEstateCommand(string Name, string Type, string Category, string Service, long OwnerId) : IRequest<AddRealEstateCommandResult>;

    public record AddRealEstateCommandResult : BaseCommandResult
    {
        public long Id { get; set; }
    }
    public class AddRealEstateCommandHandler(ApplicationDbContext _dbContext) : IRequestHandler<AddRealEstateCommand, AddRealEstateCommandResult>
    {
        public async Task<AddRealEstateCommandResult> Handle(AddRealEstateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (!Enum.TryParse<RealEstateType>(request.Type, out var realEstateType) ||
                    !Enum.TryParse<RealEstateCategory>(request.Category, out var realEstateCategory) ||
                    !Enum.TryParse<RealEstateService>(request.Service, out var realEstateService))
                {
                    return new AddRealEstateCommandResult
                    {
                        IsSuccess = false,
                        Errors = { "Invalid enum value provided for Type, Category, or Service." },
                        ErrorCode = Domain.Common.ErrorCode.InvalidDate
                    };
                }

                var realEstate = new Domain.Models.RealEstates.RealEstate
                {
                    Name = request.Name,
                    Type = realEstateType,
                    Category = realEstateCategory,
                    Service = realEstateService,
                    OwnerId = request.OwnerId
                };

                await _dbContext.RealEstates.AddAsync(realEstate, cancellationToken);

                await _dbContext.SaveChangesAsync(cancellationToken);

                return new AddRealEstateCommandResult
                {
                    IsSuccess = true,
                    Id = realEstate.Id
                };
            }
            catch (Exception ex)
            {
                return new AddRealEstateCommandResult
                {
                    IsSuccess = false,
                    Errors = { ex.Message },
                    ErrorCode = Domain.Common.ErrorCode.Error
                };
            }
        }
    }

}
