using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using FacilityLeasing.Entities;

namespace FacilityLeasing.Data.EntityConfigurations;

public class ProcessEquipmentTypeConfiguration : IEntityTypeConfiguration<ProcessEquipmentType>
{
    public void Configure(EntityTypeBuilder<ProcessEquipmentType> builder)
    {
        builder.HasKey(pe => pe.Code);
        builder.Property(pe => pe.Code).IsRequired().HasMaxLength(50);
        builder.Property(pe => pe.Name).IsRequired().HasMaxLength(200);
        builder.Property(pe => pe.Area).IsRequired();
    }
}
