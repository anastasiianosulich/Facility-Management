using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using FacilityLeasing.Entities;

namespace FacilityLeasing.Data.EntityConfigurations;

public class ProductionFacilityConfiguration : IEntityTypeConfiguration<ProductionFacility>
{
    public void Configure(EntityTypeBuilder<ProductionFacility> builder)
    {
        builder.HasKey(pf => pf.Code);
        builder.Property(pf => pf.Code).IsRequired().HasMaxLength(50);
        builder.Property(pf => pf.Name).IsRequired().HasMaxLength(200);
        builder.Property(pf => pf.StandardArea).IsRequired();
    }
}
