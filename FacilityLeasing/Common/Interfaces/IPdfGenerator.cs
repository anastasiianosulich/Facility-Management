using FacilityLeasing.Dtos;

namespace FacilityLeasing.Common.Interfaces;

public interface IPdfGenerator
{
    byte[] GenerateContractPdf(EquipmentPlacementContractDto contract);
}
