using Application.Utilities.Models;
using Infrastructure;
using MediatR;

namespace Application.Tenant.Commands.Add
{
    public record AddTenantCommand(CreateTenantDto dto) : IRequest<AddTenantCommandResult>;

    public record AddTenantCommandResult : BaseCommandResult
    {
        public long Id { get; set; }
    }
    public record CreateTenantDto(string Name, string Email, string PhoneNumber, string Address, string City, string Gender, string Nationality, string IdNumber);

    public class AddTenanrCommandHandler(ApplicationDbContext _dbContext) : IRequestHandler<AddTenantCommand, AddTenantCommandResult>
    {
        public async Task<AddTenantCommandResult> Handle(AddTenantCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var tenant = new Domain.Models.Tenants.Tenant
                {
                    Name = request.dto.Name,
                    Email = request.dto.Email,
                    PhoneNumber = request.dto.PhoneNumber,
                    Address = request.dto.Address,
                    City = request.dto.City,
                    Gender = request.dto.Gender,
                    Nationality = request.dto.Nationality,
                    IdNumber = request.dto.IdNumber
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
