using Application.Utilities.Models;
using Infrastructure;
using MediatR;

namespace Application.Tenant.Commands.Add
{
    public record AddTenantCommand(string Name, string PhoneNumber, string Address, string City, string Gender, string Nationality, string IdNumber) : IRequest<AddTenantCommandResult>;

    public record AddTenantCommandResult : BaseCommandResult
    {
        public long Id { get; set; }
    }

    public class AddTenanrCommandHandler(ApplicationDbContext _dbContext) : IRequestHandler<AddTenantCommand, AddTenantCommandResult>
    {
        public async Task<AddTenantCommandResult> Handle(AddTenantCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var tenant = new Domain.Models.Tenants.Tenant
                {
                    Name = request.Name,
                    PhoneNumber = request.PhoneNumber,
                    Address = request.Address,
                    City = request.City,
                    Gender = request.Gender,
                    Nationality = request.Nationality,
                    IdNumber = request.IdNumber
                };


                await _dbContext.Tenant.AddAsync(tenant, cancellationToken);

                await _dbContext.SaveChangesAsync(cancellationToken);

                return new AddTenantCommandResult
                {
                    IsSuccess = true,
                    Id = tenant.Id
                };

            }
            catch (Exception ex)
            {
                return new AddTenantCommandResult
                {
                    IsSuccess = false,
                    Errors = { ex.Message },
                    ErrorCode = Domain.Common.ErrorCode.Error
                };
            }
        }
    }
}
