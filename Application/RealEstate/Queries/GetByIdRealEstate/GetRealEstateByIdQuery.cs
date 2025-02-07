using Application.Services.File;
using Application.Utilities.Models;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.RealEstate.Queries.GetByIdRealEstate
{
    public record GetRealEstateByIdQuery(long Id) : IRequest<GetRealEstateByIdQueryResult>;

    public record GetRealEstateByIdQueryResult : BaseCommandResult
    {
        public RealEstatDataeDto realEstateDto { get; set; }
    }

    public record RealEstatDataeDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
        public string Service { get; set; }
        public string? Image { get; set; }
        public string? DocumentType { get; set; }
        public string? DocumentName { get; set; }
        public string? DocumentNumber { get; set; }
        public DateTime? IssueDate { get; set; }
        public string? Guard { get; set; }
        public long? GuardId { get; set; }
        public string? GuardMobile { get; set; }
        public string? AdNumber { get; set; }
        public string? ElectricityCalculation { get; set; }
        public string? GasMeter { get; set; }
        public string? WaterMeter { get; set; }
        public long OwnerId { get; set; }
    }
    public class GetRealEstateByIdQueryHandler(ApplicationDbContext _dbContext, AttachmentService _attachmentService) : IRequestHandler<GetRealEstateByIdQuery, GetRealEstateByIdQueryResult>
    {
        public async Task<GetRealEstateByIdQueryResult> Handle(GetRealEstateByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var realEstate = await _dbContext.RealEstates
                    .Include(re => re.Owner)
                    .Where(re => re.Id == request.Id)
                    .Select(re => new RealEstatDataeDto
                    {
                        Id = re.Id,
                        Name = re.Name,
                        Type = re.Type,
                        Category = re.Category,
                        Service = re.Service,
                        Image = re.Image,
                        DocumentType = re.DocumentType,
                        DocumentName = re.DocumentName,
                        DocumentNumber = re.DocumentNumber,
                        IssueDate = re.IssueDate,
                        Guard = re.Guard,
                        GuardId = re.GuardId,
                        GuardMobile = re.GuardMobile,
                        AdNumber = re.AdNumber,
                        ElectricityCalculation = re.ElectricityCalculation,
                        GasMeter = re.GasMeter,
                        WaterMeter = re.WaterMeter,
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

                var url = "";

                var profile = await _attachmentService.GetFilesUrlAsync(Path.Combine("profiles", realEstate.Id.ToString()));

                if (profile.IsSuccess && profile.Urls.Count > 0)
                {
                    realEstate.Image = profile.Urls[0];
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
