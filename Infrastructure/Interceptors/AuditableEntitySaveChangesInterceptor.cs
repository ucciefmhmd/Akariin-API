using Domain.Contractors;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Infrastructure.Common.Extensions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Domain.Models;
using Domain.Models.Contractors;

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
        foreach (var entry in context.ChangeTracker.Entries<ModelBase<Guid>>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedById = userId;
                entry.Entity.CreatedDate = DateTime.UtcNow;
            } 

            if (/*entry.State == EntityState.Added ||*/ entry.State == EntityState.Modified || entry.HasChangedOwnedEntities())
            {
                entry.Entity.ModifiedById = userId;
                entry.Entity.ModifiedDate = DateTime.UtcNow;
            }
        }
    }
}



