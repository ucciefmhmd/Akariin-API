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
                    .Select(re => new TenantDto
                    {
                        Id = re.Id,
                        Address = re.Address,
                        Birthday = re.Birthday,
                        City = re.City,
                        Email = re.Email,
                        Gender = re.Gender,
                        IdNumber = re.IdNumber,
                        Name = re.Name, 
                        Nationality = re.Nationality,
                        PhoneNumber = re.PhoneNumber
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
