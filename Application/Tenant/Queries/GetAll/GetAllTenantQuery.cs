using Application.RealEstateUnit.Queries.GetAll;
using Application.Utilities.Models;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using static Domain.Common.Enums.OwnerEnum;

namespace Application.Tenant.Queries.GetAll
{
    public record GetAllTenantQuery : IRequest<GetAllTenantQueryResult>;

    public record GetAllTenantQueryResult : BaseCommandResult
    {
        public List<TenantDto> dto { get; init; }
    }

    public record TenantDto
    {
        public long Id { get; init; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public Gender Gender { get; set; }
        public DateOnly Birthday { get; set; }
        public string Nationality { get; set; }
        public string IdNumber { get; set; }
    }

    public class GetAllTenantQueryHandler(ApplicationDbContext _dbContext) : IRequestHandler<GetAllTenantQuery, GetAllTenantQueryResult> 
    {
        public async Task<GetAllTenantQueryResult> Handle(GetAllTenantQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var tenant = await _dbContext.Tenant
                    .Select(t => new TenantDto
                    {
                        Id = t.Id,
                        Name = t.Name,
                        Email = t.Email,
                        City = t.City,
                        Address = t.Address,
                        PhoneNumber = t.PhoneNumber,
                        Gender = t.Gender,
                        Birthday = t.Birthday,
                        Nationality = t.Nationality,
                        IdNumber = t.IdNumber
                    })
                    .ToListAsync(cancellationToken);

                return new GetAllTenantQueryResult
                {
                    IsSuccess = true,
                    dto = tenant
                };
            }
            catch (Exception ex)
            {
                return new GetAllTenantQueryResult
                {
                    IsSuccess = false,
                    Errors = { ex.Message },
                    ErrorCode = Domain.Common.ErrorCode.Error
                };
            }
        }

    }


}
