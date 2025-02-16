
using Application.Tenant.Queries.GetAll;
using Application.Utilities.Models;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Tenant.Commands.Update
{
    public record UpdateTenantCommand(long Id, string Name, string PhoneNumber, string Address, string City, string Gender, string Nationality, string IdNumber) : IRequest<UpdateTenantCommandResult>;

    public record UpdateTenantCommandResult : BaseCommandResult
    {
        public long Id { get; set; }
    }


    public class UpdateTenantCommandHandler(ApplicationDbContext _dbContext) : IRequestHandler<UpdateTenantCommand, UpdateTenantCommandResult>
    {
        public async Task<UpdateTenantCommandResult> Handle(UpdateTenantCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var tenant = await _dbContext.Tenant.FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);
                
                if (tenant == null)
                {
                    return new UpdateTenantCommandResult
                    {
                        IsSuccess = false,
                        Errors = { "Tenant not found." }
                    };
                
                }
                tenant.Id = request.Id;
                tenant.Name = request.Name;
                tenant.PhoneNumber = request.PhoneNumber;
                tenant.Address = request.Address;
                tenant.City = request.City;
                tenant.Gender = request.Gender;
                tenant.Nationality = request.Nationality;
                tenant.IdNumber = request.IdNumber;

                _dbContext.Tenant.Update(tenant);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return new UpdateTenantCommandResult
                {
                    IsSuccess = true,
                    Id = tenant.Id
                };

            }
            catch (Exception ex)
            {
                return new UpdateTenantCommandResult
                {
                    IsSuccess = false,
                    Errors = { ex.Message },
                    ErrorCode = Domain.Common.ErrorCode.Error
                };
            }
        }
    }
}
