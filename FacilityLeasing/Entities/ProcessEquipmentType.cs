namespace FacilityLeasing.Entities;

public class ProcessEquipmentType
{
    public string Code { get; set; }
    public string Name { get; set; }
    public double Area { get; set; }

    public ICollection<EquipmentPlacementContract> EquipmentPlacementContracts { get; set; }
}
