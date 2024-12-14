namespace FacilityLeasing.Entities;

public class EquipmentPlacementContract
{
    public Guid Id { get; set; }
    public string ProductionFacilityCode { get; set; }
    public string ProcessEquipmentTypeCode { get; set; }
    public int EquipmentUnits { get; set; }

    public ProductionFacility ProductionFacility { get; set; }
    public ProcessEquipmentType ProcessEquipmentType { get; set; }
}
