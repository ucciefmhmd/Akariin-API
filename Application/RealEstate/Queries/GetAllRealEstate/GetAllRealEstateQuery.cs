using Application.Utilities.Models;
using Domain.Models.RealEstates;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.RealEstate.Queries.GetAllRealEstate
{
    public record GetAllRealEstateQuery : IRequest<GetAllRealEstateQueryResult>;

    public record GetAllRealEstateQueryResult : BaseCommandResult
    {
        public List<RealEstateDto> RealEstateDto { get; set; }
    }

    public record RealEstateDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
        public string Service { get; set; }
        public long OwnerId { get; set; }
    }

    public class GetAllRealEstateQueryHandler(ApplicationDbContext _dbContext) : IRequestHandler<GetAllRealEstateQuery, GetAllRealEstateQueryResult>
    {
        public async Task<GetAllRealEstateQueryResult> Handle(GetAllRealEstateQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var realEstates = await _dbContext.RealEstates
                    .Include(re => re.Owner)
                    .Select(re => new RealEstateDto
                    {
                        Id = re.Id,
                        Name = re.Name,
                        Type = re.Type.ToString(),
                        Category = re.Category.ToString(),
                        Service = re.Service.ToString(),
                        OwnerId = re.Owner.Id
                    })
                    .ToListAsync(cancellationToken);

                return new GetAllRealEstateQueryResult
                {
                    IsSuccess = true,
                    RealEstateDto = realEstates
                };
            }
            catch (Exception ex)
            {
                return new GetAllRealEstateQueryResult
                {
                    IsSuccess = false,
                    Errors = { ex.Message },
                    ErrorCode = Domain.Common.ErrorCode.Error
                };
            }
        }
    }

}
