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
using Domain.Models.MaintenanceRequests;

namespace Infrastructure;

public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options, IOptions<OperationalStoreOptions> operationalStoreOptions
        , AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor
        ) : ApiAuthorizationDbContext<ApplicationUser>(options,operationalStoreOptions)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.CheckForTrim();
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Contract>()
            .HasOne(c => c.RealEstateUnit)
            .WithMany()
            .HasForeignKey(c => c.RealEstateUnitId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Contract>()
            .HasOne(c => c.RealEstate)
            .WithMany() 
            .HasForeignKey(c => c.RealEstateId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<RealEstate>()
            .HasOne(c => c.Owner)
            .WithMany(p => p.OwnerRealEstate)
            .HasForeignKey(c => c.OwnerId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<RealEstateUnit>()
            .HasOne(c => c.Tenant)
            .WithMany(p => p.TanentRealEstateUnit)
            .HasForeignKey(c => c.TenantId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Contract>()
            .HasOne(c => c.Marketer)
            .WithMany(p => p.MarketerContract)
            .HasForeignKey(c => c.MarketerId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Contract>()
            .HasOne(c => c.Tenant)
            .WithMany(p => p.TanentContract)
            .HasForeignKey(c => c.TenantId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Bill>()
            .HasOne(c => c.Marketer)
            .WithMany(p => p.MarketerBill)
            .HasForeignKey(c => c.MarketerId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Bill>()
            .HasOne(c => c.Tenant)
            .WithMany(p => p.TanentBill)
            .HasForeignKey(c => c.TenantId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<MaintenanceRequest>()
            .HasOne(c => c.Tenant)
            .WithMany(p => p.TanentMaintenanceRequest)
            .HasForeignKey(c => c.TenantId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<MaintenanceRequest>()
            .HasOne(c => c.Member)
            .WithMany(p => p.MemberMaintenanceRequest)
            .HasForeignKey(c => c.MemberId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<MaintenanceRequest>()
            .HasOne(c => c.RealEstate)
            .WithMany()
            .HasForeignKey(c => c.RealEstateId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<MaintenanceRequest>()
            .HasOne(c => c.RealEstateUnit)
            .WithMany()
            .HasForeignKey(c => c.RealEstateUnitId)
            .OnDelete(DeleteBehavior.NoAction);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging();

        optionsBuilder.AddInterceptors(auditableEntitySaveChangesInterceptor);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        //await _mediator.DispatchDomainEvents(this);

        return await base.SaveChangesAsync(cancellationToken);
    }

    public DbSet<Member> Members { get; set; }

    public DbSet<RealEstate> RealEstates { get; set; }

    public DbSet<RealEstateUnit> RealEstateUnits { get; set; }

    public DbSet<Contract> Contracts { get; set; }

    public DbSet<Bill> Bills { get; set; }

    public DbSet<Page> Pages { get; set; }

    public DbSet<UserPageRole> UserPageRoles { get; set; }

    public DbSet<MaintenanceRequest> MaintenanceRequests { get; set; }

}
