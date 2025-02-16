using Domain.Contractors;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Infrastructure.Common.Extensions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Infrastructure.Interceptors;

public class AuditableEntitySaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly IHttpContextAccessor _httpContext;

    public AuditableEntitySaveChangesInterceptor(IHttpContextAccessor httpContext)
    {
        _httpContext = httpContext;
    }
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void UpdateEntities(DbContext? context)
    {
        if (context == null) return;

        var userId = _httpContext.HttpContext?.User?.FindFirstValue(JwtRegisteredClaimNames.Jti);

        foreach (var entry in context.ChangeTracker.Entries().Where(e => e.Entity is ModelBase<long> || e.Entity is ModelBase<Guid>))
        {
            if (entry.Entity is ModelBase<long> longEntity)
            {
                if (entry.State == EntityState.Added)
                {
                    longEntity.CreatedById = userId;
                    longEntity.CreatedDate = DateTime.UtcNow;
                }

                if (entry.State == EntityState.Modified || entry.HasChangedOwnedEntities())
                {
                    longEntity.ModifiedById = userId;
                    longEntity.ModifiedDate = DateTime.UtcNow;
                }
            }
            else if (entry.Entity is ModelBase<Guid> guidEntity)
            {
                if (entry.State == EntityState.Added)
                {
                    guidEntity.CreatedById = userId;
                    guidEntity.CreatedDate = DateTime.UtcNow;
                }

                if (entry.State == EntityState.Modified || entry.HasChangedOwnedEntities())
                {
                    guidEntity.ModifiedById = userId;
                    guidEntity.ModifiedDate = DateTime.UtcNow;
                }
            }
        }

    }
}



