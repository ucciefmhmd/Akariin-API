using Domain.Contractors;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Infrastructure.Common.Extensions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Infrastructure.Interceptors;

public class AuditableEntitySaveChangesInterceptor(IHttpContextAccessor _httpContext) : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        HandleEntityUpdates(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        HandleEntityUpdates(eventData.Context);

        //UpdateEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void HandleEntityUpdates(DbContext? context)
    {
        if (context == null) return;

        var httpContext = _httpContext.HttpContext;

        var user = httpContext?.User;

        var userId = user?.FindFirstValue(JwtRegisteredClaimNames.Jti);

        if (user == null || string.IsNullOrEmpty(userId)) return;

        bool isAdmin = user.IsInRole("Admin") || user.IsInRole("SubAdmin");

        // Check if any tracked entity contains a UserId property and if it's not null
        bool containsUserId = context.ChangeTracker.Entries()
                                                    .Any(e =>
                                                        e.Entity?.GetType().GetProperty("CreatedById") != null &&
                                                        e.Entity.GetType().GetProperty("CreatedById")?.GetValue(e.Entity) is Guid userIdValue &&
                                                        userIdValue != Guid.Empty);

        // If user is Admin and request contains UserId, skip updating entities
        if (isAdmin && containsUserId)
        {
            return;
        }

        UpdateEntities(context, userId);
    }

    //public void UpdateEntities(DbContext? context)
    //{
    //    if (context == null) return;

    //    var userId = _httpContext.HttpContext?.User?.FindFirstValue(JwtRegisteredClaimNames.Jti);

    //    foreach (var entry in context.ChangeTracker.Entries().Where(e => e.Entity is ModelBase<long> || e.Entity is ModelBase<Guid>))
    //    {
    //        if (entry.Entity is ModelBase<long> longEntity)
    //        {
    //            if (entry.State == EntityState.Added)
    //            {
    //                longEntity.CreatedById = userId;
    //                longEntity.CreatedDate = DateTime.UtcNow;
    //            }

    //            if (entry.State == EntityState.Modified || entry.HasChangedOwnedEntities())
    //            {
    //                longEntity.ModifiedById = userId;
    //                longEntity.ModifiedDate = DateTime.UtcNow;
    //            }
    //        }
    //        else if (entry.Entity is ModelBase<Guid> guidEntity)
    //        {
    //            if (entry.State == EntityState.Added)
    //            {
    //                guidEntity.CreatedById = userId;
    //                guidEntity.CreatedDate = DateTime.UtcNow;
    //            }

    //            if (entry.State == EntityState.Modified || entry.HasChangedOwnedEntities())
    //            {
    //                guidEntity.ModifiedById = userId;
    //                guidEntity.ModifiedDate = DateTime.UtcNow;
    //            }
    //        }
    //    }

    //}


    private void UpdateEntities(DbContext context, string userId)
    {
        foreach (var entry in context.ChangeTracker.Entries())
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



