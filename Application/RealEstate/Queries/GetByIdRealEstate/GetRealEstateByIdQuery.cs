using Application.RealEstate.Queries.GetAllRealEstate;
using Application.Utilities.Models;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.RealEstate.Queries.GetByIdRealEstate
{
    public record GetRealEstateByIdQuery(long Id) : IRequest<GetRealEstateByIdQueryResult>;

    public record GetRealEstateByIdQueryResult : BaseCommandResult
    {
        public RealEstateDto realEstateDto { get; set; }
    }
    public class GetRealEstateByIdQueryHandler(ApplicationDbContext _dbContext) : IRequestHandler<GetRealEstateByIdQuery, GetRealEstateByIdQueryResult>
    {
        public async Task<GetRealEstateByIdQueryResult> Handle(GetRealEstateByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var realEstate = await _dbContext.RealEstates
                    .Include(re => re.Owner)
                    .Where(re => re.Id == request.Id)
                    .Select(re => new RealEstateDto
                    {
                        Id = re.Id,
                        Name = re.Name,
                        Type = re.Type,
                        Category = re.Category,
                        Service = re.Service,
                        OwnerId = re.Owner.Id
                    })
                    .FirstOrDefaultAsync(cancellationToken);

                if (realEstate == null)
                {
                    return new GetRealEstateByIdQueryResult
                    {
                        IsSuccess = false,
                        Errors = { "Real estate not found." }
                    };
                }

                return new GetRealEstateByIdQueryResult
                {
                    IsSuccess = true,
                    realEstateDto = realEstate
                };
            }
            catch (Exception ex)
            {
                return new GetRealEstateByIdQueryResult
                {
                    IsSuccess = false,
                    Errors = { ex.Message },
                    ErrorCode = Domain.Common.ErrorCode.Error
                };
            }
        }
    }

}
