using Application.RealEstateUnit.Queries.GetAll;
using Application.Tenant.Queries.GetAll;
using Application.Utilities.Models;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace Application.Tenant.Queries.GetById
{
    public record GetTenantByIdQuery(long Id) : IRequest<GetTenantByIdQueryResult>;

    public record GetTenantByIdQueryResult : BaseCommandResult
    {
        public TenantDto dto { get; set; }
    }

    public class GetTenantByIdQueryHandler(ApplicationDbContext _dbContext) : IRequestHandler<GetTenantByIdQuery, GetTenantByIdQueryResult>
    {
        public async Task<GetTenantByIdQueryResult> Handle(GetTenantByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var tenant = await _dbContext.Tenant
                    .Select(t => new TenantDto
                    {
                        Id = t.Id,
                        Address = t.Address,
                        City = t.City,
                        Gender = t.Gender,
                        IdNumber = t.IdNumber,
                        Name = t.Name, 
                        Nationality = t.Nationality,
                        PhoneNumber = t.PhoneNumber,
                        CreatedBy = t.CreatedBy != null ? new CreatedByVM { Name = t.CreatedBy.Name, Id = t.CreatedBy.Id } : null,
                        ModifiedBy = t.ModifiedBy != null ? new CreatedByVM { Name = t.ModifiedBy.Name, Id = t.ModifiedBy.Id } : null,
                        CreatedDate = t.CreatedDate,
                        ModifiedDate = t.ModifiedDate,

                    })
                    .FirstOrDefaultAsync(cancellationToken);

                if (tenant == null)
                {
                    return new GetTenantByIdQueryResult
                    {
                        IsSuccess = false,
                        Errors = { "Tenant unit not found." }
                    };
                }

                return new GetTenantByIdQueryResult
                {
                    IsSuccess = true,
                    dto = tenant
                };
            }
            catch (Exception ex)
            {
                return new GetTenantByIdQueryResult
                {
                    IsSuccess = false,
                    Errors = { ex.Message },
                    ErrorCode = Domain.Common.ErrorCode.Error
                };
            }
        }
    }



}
