using AutoMapper;
using FacilityLeasing.Dtos;
using FacilityLeasing.Entities;

namespace FacilityLeasing.MappingProfiles;

public sealed class FacilityManagementProfile : Profile
{
    public FacilityManagementProfile()
    {
        CreateMap<EquipmentPlacementContract, EquipmentPlacementContractDto>()
            .ForMember(x => x.ProcessEquipmentTypeName, opt => opt.MapFrom(src => src.ProcessEquipmentType.Name))
            .ForMember(x => x.ProductionFacilityName, opt => opt.MapFrom(src => src.ProductionFacility.Name));

        CreateMap<CreateEquipmentPlacementContractDto, EquipmentPlacementContract>();
    }
}