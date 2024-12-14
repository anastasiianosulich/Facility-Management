using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using FacilityLeasing.Entities;

namespace FacilityLeasing.Data.EntityConfigurations;

public class EquipmentPlacementContractConfiguration : IEntityTypeConfiguration<EquipmentPlacementContract>
{
    public void Configure(EntityTypeBuilder<EquipmentPlacementContract> builder)
    {
        builder.HasKey(epc => epc.Id);

        builder.HasOne(epc => epc.ProductionFacility)
               .WithMany(pf => pf.EquipmentPlacementContracts)
               .HasForeignKey(epc => epc.ProductionFacilityCode);

        builder.HasOne(epc => epc.ProcessEquipmentType)
               .WithMany(pe => pe.EquipmentPlacementContracts)
               .HasForeignKey(epc => epc.ProcessEquipmentTypeCode);
    }
}
