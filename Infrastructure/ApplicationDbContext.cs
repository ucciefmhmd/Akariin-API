using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Domain.Identity;
using System.Reflection;
using Infrastructure.Common.Extensions;
using Infrastructure.Interceptors;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Duende.IdentityServer.EntityFramework.Options;
using Domain.Models.Owners;
using Domain.Models.RealEstates;
using Domain.Models.RealEstateUnits;
using Domain.Models.Tenants;
using Domain.Models.Bills;
using Domain.Models.Contracts;


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

        modelBuilder.Entity<Contract>()
            .HasOne(c => c.RealEstateUnit)
            .WithMany()
            .HasForeignKey(c => c.RealEstateUnitId)
            .OnDelete(DeleteBehavior.Cascade); // Allow cascade delete here

        // Configure Tenant relationship with restrict delete
        modelBuilder.Entity<Contract>()
            .HasOne(c => c.Tenant)
            .WithMany()
            .HasForeignKey(c => c.TenantId)
            .OnDelete(DeleteBehavior.Restrict);
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

    public DbSet<Owner> Owners { get; set; }
    public DbSet<RealEstate> RealEstates { get; set; }
    public DbSet<RealEstateUnit> RealEstateUnits { get; set; }
    public DbSet<Tenant> Tenant { get; set; }
    public DbSet<Contract> Contracts { get; set; }
    public DbSet<Bill> Bills { get; set; }

}
