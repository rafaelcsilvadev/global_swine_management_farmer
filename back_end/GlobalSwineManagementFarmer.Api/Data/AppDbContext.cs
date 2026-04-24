using GlobalSwineManagementFarmer.Api.src.Auth.Entities;
using GlobalSwineManagementFarmer.Api.src.Batch.Entities;
using GlobalSwineManagementFarmer.Api.src.Warehouse.Entities;
using GlobalSwineManagementFarmer.Api.src.PigBirth.Entities;
using GlobalSwineManagementFarmer.Api.src.Consumption.Entities;
using GlobalSwineManagementFarmer.Api.src.HealthMedication.Entities;
using GlobalSwineManagementFarmer.Api.src.Symptom.Entities;
using GlobalSwineManagementFarmer.Api.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace GlobalSwineManagementFarmer.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<FarmerEntity> Farmers { get; set; } = null!;
    public DbSet<RuleEntity> Rules { get; set; } = null!;
    public DbSet<BatchEntity> Batches { get; set; } = null!;
    public DbSet<WarehouseEntity> Warehouses { get; set; } = null!;
    public DbSet<WarehouseTitleEntity> WarehouseTitles { get; set; } = null!;
    public DbSet<PigBirthEntity> PigBirths { get; set; } = null!;
    public DbSet<ConsumptionEntity> Consumptions { get; set; } = null!;
    public DbSet<HealthMedicationEntity> HealthMedications { get; set; } = null!;
    public DbSet<SymptomEntity> Symptoms { get; set; } = null!;
    public DbSet<SymptomObservedEntity> SymptomsObserved { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        foreach (var type in modelBuilder.Model.GetEntityTypes()
            .Select(e => e.ClrType)
            .Where(t => t != null && typeof(BaseEntity).IsAssignableFrom(t)))
        {
            modelBuilder.Entity(type!)
                .Property("Id")
                .HasDefaultValueSql("gen_random_uuid()")
                .ValueGeneratedOnAdd();

            modelBuilder.Entity(type!)
                .Property("CreatedAt")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity(type!)
                .Property("UpdatedAt")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }

        modelBuilder.Entity<FarmerEntity>().HasQueryFilter(f => f.DeletedAt == null);
        modelBuilder.Entity<RuleEntity>().HasQueryFilter(r => r.DeletedAt == null);
        modelBuilder.Entity<BatchEntity>().HasQueryFilter(b => b.DeletedAt == null);
        modelBuilder.Entity<WarehouseEntity>().HasQueryFilter(w => w.DeletedAt == null);
        modelBuilder.Entity<WarehouseTitleEntity>().HasQueryFilter(wt => wt.DeletedAt == null);
        modelBuilder.Entity<PigBirthEntity>().HasQueryFilter(pb => pb.DeletedAt == null);
        modelBuilder.Entity<ConsumptionEntity>().HasQueryFilter(c => c.DeletedAt == null);
        modelBuilder.Entity<HealthMedicationEntity>().HasQueryFilter(hm => hm.DeletedAt == null);
        modelBuilder.Entity<SymptomEntity>().HasQueryFilter(s => s.DeletedAt == null);

        modelBuilder.Entity<SymptomObservedEntity>()
            .HasKey(so => new { so.SymptomId, so.HealthMedicationId });

        modelBuilder.Entity<SymptomObservedEntity>()
            .HasOne(so => so.Symptom)
            .WithMany(s => s.SymptomsObserved)
            .HasForeignKey(so => so.SymptomId);

        modelBuilder.Entity<SymptomObservedEntity>()
            .HasOne(so => so.HealthMedication)
            .WithMany(hm => hm.SymptomsObserved)
            .HasForeignKey(so => so.HealthMedicationId);

        modelBuilder.Entity<SymptomObservedEntity>()
            .HasQueryFilter(so => so.HealthMedication.DeletedAt == null);

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
