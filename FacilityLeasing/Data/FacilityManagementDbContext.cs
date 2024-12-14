using Microsoft.EntityFrameworkCore;
using FacilityLeasing.Data.EntityConfigurations;
using FacilityLeasing.Entities;

namespace FacilityLeasing.Data;

public class FacilityManagementDbContext : DbContext
{
    public FacilityManagementDbContext(DbContextOptions<FacilityManagementDbContext> options)
           : base(options)
    {
    }

    public FacilityManagementDbContext()
    {
    }

    public DbSet<ProductionFacility> ProductionFacilities { get; set; }
    public DbSet<ProcessEquipmentType> ProcessEquipmentTypes { get; set; }
    public DbSet<EquipmentPlacementContract> EquipmentPlacementContracts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ProductionFacilityConfiguration());
        modelBuilder.ApplyConfiguration(new ProcessEquipmentTypeConfiguration());
        modelBuilder.ApplyConfiguration(new EquipmentPlacementContractConfiguration());
    }
}
