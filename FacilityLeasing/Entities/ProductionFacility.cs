namespace FacilityLeasing.Entities;

public class ProductionFacility
{
    public string Code { get; set; }
    public string Name { get; set; }
    public double StandardArea { get; set; }

    public ICollection<EquipmentPlacementContract> EquipmentPlacementContracts { get; set; }
}
