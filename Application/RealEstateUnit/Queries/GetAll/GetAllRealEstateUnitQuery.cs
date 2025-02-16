using Application.Services.File;
using Application.Utilities.Extensions;
using Application.Utilities.Filter;
using Application.Utilities.Models;
using Application.Utilities.Sort;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.RealEstateUnit.Queries.GetAll
{
    public record GetAllRealEstateUnitQuery : BasePaginatedQuery, IRequest<GetAllRealEstateUnitQueryResult>
    {
        public string? UserId { get; set; }
    }

    public record GetAllRealEstateUnitQueryResult : BaseCommandResult
    {
        public BasePaginatedList<RealEstateUnitDto> dto { get; set; }
    }

    public record RealEstateUnitDto
    {
        public long Id { get; set; }
        public string AnnualRent { get; set; }
        public string Area { get; set; }
        public string Floor { get; set; }
        public string UnitNumber { get; set; }
        public string NumOfRooms { get; set; }
        public string Type { get; set; }
        public string? Image { get; set; }
        public string Status { get; set; }
        public long? TenantId { get; set; }
        public long RealEstateId { get; set; }
        public CreatedByVM CreatedBy { get; set; }
        public CreatedByVM ModifiedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; } 

    }
    public record CreatedByVM
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
    }

    public class GetAllRealEstateUnitQueryHandler(ApplicationDbContext _dbContext, AttachmentService _attachmentService) : IRequestHandler<GetAllRealEstateUnitQuery, GetAllRealEstateUnitQueryResult>
    {
        public async Task<GetAllRealEstateUnitQueryResult> Handle(GetAllRealEstateUnitQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var realEstateUnits = await _dbContext.RealEstateUnits
                                                 .Include(re => re.Tenant)
                                                 .Search(request.SearchTerm)
                                                 .Where(i => i.CreatedById == request.UserId || request.UserId == null)
                                                 .Select(re => new RealEstateUnitDto
                                                 {
                                                     Id = re.Id,
                                                     AnnualRent = re.AnnualRent,
                                                     Area = re.Area,
                                                     Floor = re.Floor,
                                                     UnitNumber = re.UnitNumber,
                                                     NumOfRooms = re.NumOfRooms,
                                                     Type = re.Type,
                                                     Status = re.Status,
                                                     TenantId = re.TenantId,
                                                     RealEstateId = re.RealEstateId,
                                                     CreatedBy = re.CreatedBy != null ? new CreatedByVM { Name = re.CreatedBy.Name, Id = re.CreatedBy.Id } : null,
                                                     ModifiedBy = re.ModifiedBy != null ? new CreatedByVM { Name = re.ModifiedBy.Name, Id = re.ModifiedBy.Id } : null,
                                                     CreatedDate = re.CreatedDate,
                                                     ModifiedDate = re.ModifiedDate,
                                                 })
                                                 .Filter(request.Filters)
                                                 .Sort(request.Sorts ?? new List<SortedQuery>() { new SortedQuery() { PropertyName = "Name", Direction = SortDirection.ASC } })
                                                 .ToPaginatedListAsync(request.PageNumber, request.PageSize);

                foreach (var realEstate in realEstateUnits.Items)
                {
                    var profile = await _attachmentService.GetFilesUrlAsync(Path.Combine("profiles", realEstate.Id.ToString()));

                    if (profile.IsSuccess && profile.Urls.Count > 0)
                    {
                        realEstate.Image = profile.Urls[0];
                    }
                }

                return new GetAllRealEstateUnitQueryResult
                {
                    IsSuccess = true,
                    dto = realEstateUnits
                };
            }
            catch (Exception ex)
            {
                return new GetAllRealEstateUnitQueryResult
                {
                    IsSuccess = false,
                    Errors = { ex.Message },
                    ErrorCode = Domain.Common.ErrorCode.Error
                };
            }
        }
    }
}
