using FacilityLeasing.Data;
using FacilityLeasing.Entities;

namespace FacilityLeasing.Extensions;

public static class SeedExtensions
{
    public static async Task SeedProductionFacilities(this IApplicationBuilder applicationBuilder)
    {
        using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
        {
            var context = serviceScope.ServiceProvider.GetRequiredService<FacilityManagementDbContext>();

            if (!context.ProductionFacilities.Any())
            {
                context.ProductionFacilities.AddRange(
                [
                    new ProductionFacility { Code = "PF001", Name = "Facility A", StandardArea = 1000 },
                    new ProductionFacility { Code = "PF002", Name = "Facility B", StandardArea = 2000 },
                    new ProductionFacility { Code = "PF003", Name = "Facility C", StandardArea = 1500 },
                    new ProductionFacility { Code = "PF004", Name = "Facility D", StandardArea = 2500 }
                ]);
            }

            await context.SaveChangesAsync();
        }
    }

    public static async Task SeedProcessEquipmentTypes(this IApplicationBuilder applicationBuilder)
    {
        using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
        {
            var context = serviceScope.ServiceProvider.GetRequiredService<FacilityManagementDbContext>();

            if (!context.ProcessEquipmentTypes.Any())
            {
                context.ProcessEquipmentTypes.AddRange(
                [
                    new ProcessEquipmentType { Code = "EQ001", Name = "Equipment A", Area = 50 },
                    new ProcessEquipmentType { Code = "EQ002", Name = "Equipment B", Area = 75 },
                    new ProcessEquipmentType { Code = "EQ003", Name = "Equipment C", Area = 100 },
                    new ProcessEquipmentType { Code = "EQ004", Name = "Equipment D", Area = 120 }
                ]);
            }

            await context.SaveChangesAsync();
        }
    }
}