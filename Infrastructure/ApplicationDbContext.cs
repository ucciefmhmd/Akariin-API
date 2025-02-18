using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Domain.Identity;
using System.Reflection;
using Infrastructure.Common.Extensions;
using Infrastructure.Interceptors;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Duende.IdentityServer.EntityFramework.Options;
using Domain.Models.RealEstates;
using Domain.Models.RealEstateUnits;
using Domain.Models.Bills;
using Domain.Models.Contracts;
using Domain.Models.RoleSysem;
using Domain.Models.Members;
using Domain.Models.Tenants;


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

        // Configure RealEstateUnit with cascade delete
        modelBuilder.Entity<Contract>()
            .HasOne(c => c.RealEstateUnit)
            .WithMany()
            .HasForeignKey(c => c.RealEstateUnitId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure RealEstate with no action on delete
        modelBuilder.Entity<Contract>()
            .HasOne(c => c.RealEstate)
            .WithMany()
            .HasForeignKey(c => c.RealEstateId)
            .OnDelete(DeleteBehavior.NoAction);

        // Configure Owner relationship
        modelBuilder.Entity<RealEstate>()
            .HasOne(c => c.Owner)
            .WithMany(p => p.OwnerRealEstate)
            .HasForeignKey(c => c.OwnerId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure Tenant relationship
        modelBuilder.Entity<RealEstateUnit>()
            .HasOne(c => c.Tenant)
            .WithMany(p => p.TanentRealEstateUnit)
            .HasForeignKey(c => c.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        // Correct Marketer relationship with restrict delete
        modelBuilder.Entity<Contract>()
            .HasOne(c => c.Marketer)
            .WithMany(p => p.MarketerContract)
            .HasForeignKey(c => c.MarketerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Correct Tenant relationship with restrict delete
        modelBuilder.Entity<Contract>()
            .HasOne(c => c.Tenant)
            .WithMany(p => p.TanentContract)
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

    //public DbSet<Owner> Owners { get; set; }

    public DbSet<Member> Members { get; set; }

    public DbSet<RealEstate> RealEstates { get; set; }

    public DbSet<RealEstateUnit> RealEstateUnits { get; set; }

    public DbSet<Tenant> Tenant { get; set; }

    public DbSet<Contract> Contracts { get; set; }

    public DbSet<Bill> Bills { get; set; }

    public DbSet<Page> Pages { get; set; }

    public DbSet<UserPageRole> UserPageRoles { get; set; }

}
