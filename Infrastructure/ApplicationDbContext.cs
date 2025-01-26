
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Domain.Identity;
using System.Reflection;
using Infrastructure.Common.Extensions;
using Infrastructure.Interceptors;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Duende.IdentityServer.EntityFramework.Options;



namespace Infrastructure;

public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
{
    private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options, IOptions<OperationalStoreOptions> operationalStoreOptions
        , AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor
        ) : base(options,operationalStoreOptions)
    {
        this._auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
        
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());


        modelBuilder.CheckForTrim();


        base.OnModelCreating(modelBuilder);
    }
    

    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        //await _mediator.DispatchDomainEvents(this);

        return await base.SaveChangesAsync(cancellationToken);
    }

}
