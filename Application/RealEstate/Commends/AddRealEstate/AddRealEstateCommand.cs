using Application.Utilities.Models;
using Infrastructure;
using MediatR;
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
                var realEstate = new Domain.Models.RealEstates.RealEstate
                {
                    Name = request.Name,
                    Type = request.Type,
                    Category = request.Category,
                    Service = request.Service,
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
