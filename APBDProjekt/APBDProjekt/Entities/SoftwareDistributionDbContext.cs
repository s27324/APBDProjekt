using APBDProjekt.Entities.Configs;
using Microsoft.EntityFrameworkCore;

namespace APBDProjekt.Entities;

public class SoftwareDistributionDbContext: DbContext
{
    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<Client> Clients { get; set; }
    public virtual DbSet<Company> Companies { get; set; }
    public virtual DbSet<Contract> Contracts { get; set; }
    public virtual DbSet<Discount> Discounts { get; set; }
    public virtual DbSet<PrivateClient> PrivateClients { get; set; }
    public virtual DbSet<SoftwareSystem> SoftwareSystems { get; set; }
    public virtual DbSet<Version> Versions { get; set; }
    public virtual DbSet<SoftwareSystemVersion> SoftwareSystemVersions { get; set; }
    public virtual DbSet<AppUser?> AppUsers { get; set; }
    public virtual DbSet<Role> Roles { get; set; }

    protected SoftwareDistributionDbContext()
    {
    }

    public SoftwareDistributionDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CategoryEfConfiguration).Assembly);
    }
}