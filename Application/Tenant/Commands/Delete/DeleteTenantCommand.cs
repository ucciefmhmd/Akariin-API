using Application.Utilities.Models;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Tenant.Commands.Delete
{
    public record DeleteTenantCommand(long id) : IRequest<DeleteTenantCommandResult>;

    public record DeleteTenantCommandResult : BaseCommandResult
    {
    }

    public class DeleteTenantCommandHandler(ApplicationDbContext _dbContext) : IRequestHandler<DeleteTenantCommand, DeleteTenantCommandResult>
    {
        public async Task<DeleteTenantCommandResult> Handle(DeleteTenantCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var tenant = await _dbContext.Tenant.FirstOrDefaultAsync(t => t.Id == request.id, cancellationToken);
                
                if (tenant == null)
                {
                    return new DeleteTenantCommandResult
                    {
                        IsSuccess = false,
                        Errors = { "Tenant not found." }
                    };
                }

                _dbContext.Tenant.Remove(tenant);

                await _dbContext.SaveChangesAsync(cancellationToken);

                return new DeleteTenantCommandResult
                {
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new DeleteTenantCommandResult
                {
                    IsSuccess = false,
                    Errors = { ex.Message },
                    ErrorCode = Domain.Common.ErrorCode.Error
                };
            }
        }
    }
}
