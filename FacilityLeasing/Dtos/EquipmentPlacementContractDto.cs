namespace FacilityLeasing.Dtos;

public class EquipmentPlacementContractDto
{
    public Guid Id { get; set; }

    public string ProductionFacilityName { get; set; }

    public string ProcessEquipmentTypeName { get; set; }

    public int EquipmentUnits { get; set; }
}