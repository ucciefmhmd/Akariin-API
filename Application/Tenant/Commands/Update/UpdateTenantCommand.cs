
using Application.Tenant.Queries.GetAll;
using Application.Utilities.Models;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Tenant.Commands.Update
{
    public record UpdateTenantCommand(TenantDto dto) : IRequest<UpdateTenantCommandResult>;

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
                var tenant = await _dbContext.Tenant.FirstOrDefaultAsync(t => t.Id == request.dto.Id, cancellationToken);
                
                if (tenant == null)
                {
                    return new UpdateTenantCommandResult
                    {
                        IsSuccess = false,
                        Errors = { "Tenant not found." }
                    };
                
                }
                tenant.Id = request.dto.Id;
                tenant.Name = request.dto.Name;
                tenant.Email = request.dto.Email;
                tenant.PhoneNumber = request.dto.PhoneNumber;
                tenant.Address = request.dto.Address;
                tenant.City = request.dto.City;
                tenant.Birthday = request.dto.Birthday;
                tenant.Gender = request.dto.Gender;
                tenant.Nationality = request.dto.Nationality;
                tenant.IdNumber = request.dto.IdNumber;

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
