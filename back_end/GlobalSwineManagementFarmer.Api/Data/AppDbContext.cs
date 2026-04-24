using GlobalSwineManagementFarmer.Api.src.Auth.Entities;
using GlobalSwineManagementFarmer.Api.src.Batch.Entities;
using GlobalSwineManagementFarmer.Api.src.Warehouse.Entities;
using GlobalSwineManagementFarmer.Api.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace GlobalSwineManagementFarmer.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<FarmerEntity> Farmers { get; set; } = null!;
    public DbSet<RuleEntity>   Rules   { get; set; } = null!;
    public DbSet<BatchEntity>  Batches { get; set; } = null!;
    public DbSet<WarehouseEntity> Warehouses { get; set; } = null!;
    public DbSet<WarehouseTitleEntity> WarehouseTitles { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder.Entity(entityType.ClrType)
                    .Property("Id")
                    .HasDefaultValueSql("gen_random_uuid()");

                modelBuilder.Entity(entityType.ClrType)
                    .Property("CreatedAt")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                modelBuilder.Entity(entityType.ClrType)
                    .Property("UpdatedAt")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            }
        }

        modelBuilder.Entity<FarmerEntity>().HasQueryFilter(f => f.DeletedAt == null);
        modelBuilder.Entity<RuleEntity>().HasQueryFilter(r => r.DeletedAt == null);
        modelBuilder.Entity<BatchEntity>().HasQueryFilter(b => b.DeletedAt == null);
        modelBuilder.Entity<WarehouseEntity>().HasQueryFilter(w => w.DeletedAt == null);
        modelBuilder.Entity<WarehouseTitleEntity>().HasQueryFilter(wt => wt.DeletedAt == null);

        modelBuilder.Entity<FarmerEntity>()
            .HasIndex(f => f.Email)
            .IsUnique();

        modelBuilder.Entity<BatchEntity>()
            .HasIndex(b => b.Code)
            .IsUnique();

        modelBuilder.Entity<WarehouseEntity>()
            .HasIndex(w => w.Code)
            .IsUnique();

        modelBuilder.Entity<WarehouseTitleEntity>()
            .HasIndex(wt => wt.Title)
            .IsUnique();
    }
}
