namespace FacilityLeasing.Dtos;

public class CreateEquipmentPlacementContractDto
{
    public string ProductionFacilityCode { get; set; }

    public string ProcessEquipmentTypeCode { get; set; }

    public int EquipmentUnits { get; set; }
}
