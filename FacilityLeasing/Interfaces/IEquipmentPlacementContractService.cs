using FacilityLeasing.Dtos;

namespace FacilityLeasing.Interfaces;

public interface IEquipmentPlacementContractService
{
    Task<EquipmentPlacementContractDto> CreateAsync(CreateEquipmentPlacementContractDto createContractDto);
    Task<IEnumerable<EquipmentPlacementContractDto>> GetAllAsync();
}
