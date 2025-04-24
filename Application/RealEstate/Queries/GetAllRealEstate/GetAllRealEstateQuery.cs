using Application.Services.File;
using Application.Utilities.Extensions;
using Application.Utilities.Filter;
using Application.Utilities.Models;
using Application.Utilities.Sort;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.RealEstate.Queries.GetAllRealEstate
{
    public record GetAllRealEstateQuery : BasePaginatedQuery, IRequest<GetAllRealEstateQueryResult>
    {
        public string? UserId { get; set; }
    }

    public record GetAllRealEstateQueryResult : BaseCommandResult
    {
        public BasePaginatedList<RealEstateDto> RealEstateDto { get; set; }
    }

    public record RealEstateDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
        public string Service { get; set; }
        public string? Image { get; set; }
        public long CountRealEstateUnit { get; set; }
        public long CountContracts { get; set; }
        public decimal ContractSumSalary { get; set; }
        public string Status { get; set; }
        public long OwnerId { get; set; }
        public CreatedByVM CreatedBy { get; set; }
        public CreatedByVM ModifiedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

    }

    public record CreatedByVM
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
    }

    public class GetAllRealEstateQueryHandler(ApplicationDbContext _dbContext, AttachmentService _attachmentService) : IRequestHandler<GetAllRealEstateQuery, GetAllRealEstateQueryResult>
    {
        public async Task<GetAllRealEstateQueryResult> Handle(GetAllRealEstateQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var realEstates = await _dbContext.RealEstates
                    .Include(re => re.Owner)
                    .Include(re => re.Contracts)
                    .Search(request.SearchTerm)
                    .Where(re => !re.IsDeleted && (re.CreatedById == request.UserId || request.UserId == null))
                    .Select(re => new RealEstateDto
                    {
                        Id = re.Id,
                        Name = re.Name,
                        Type = re.Type,
                        Category = re.Category,
                        Service = re.Service,
                        CountRealEstateUnit = re.RealEstateUnits.Count,
                        CountContracts = _dbContext.Contracts.Count(c => c.RealEstateId == re.Id && c.IsActive),
                        ContractSumSalary = _dbContext.Contracts.Where(c => c.RealEstateId == re.Id && c.IsActive).Sum(c => (decimal?)c.PaymentAmount) ?? 0m,
                        OwnerId = re.Owner.Id,
                        CreatedBy = re.CreatedBy != null ? new CreatedByVM { Name = re.CreatedBy.Name, Id = re.CreatedBy.Id } : null,
                        ModifiedBy = re.ModifiedBy != null ? new CreatedByVM { Name = re.ModifiedBy.Name, Id = re.ModifiedBy.Id } : null,
                        CreatedDate = re.CreatedDate,
                        ModifiedDate = re.ModifiedDate,
                        Status = re.Status,
                        Image = re.Image                      
                    })
                    .Filter(request.Filters)
                    .Sort(request.Sorts ?? new () { new SortedQuery() { PropertyName = "Name", Direction = SortDirection.ASC } })
                    .ToPaginatedListAsync(request.PageNumber, request.PageSize);

                foreach (var realEstate in realEstates.Items)
                {
                    var profile = await _attachmentService.GetFilesUrlAsync(Path.Combine("profiles", realEstate.Id.ToString()));

                    if (profile.IsSuccess && profile.Urls.Count > 0)
                    {
                        realEstate.Image = profile.Urls[0];
                    }
                }

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