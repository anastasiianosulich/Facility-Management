using FacilityLeasing.Data;
using FacilityLeasing.Exceptions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using FacilityLeasing.Dtos;
using FacilityLeasing.Entities;
using FacilityLeasing.Interfaces;

namespace FacilityLeasing.Services;

public class EquipmentPlacementContractService : IEquipmentPlacementContractService
{
    private readonly FacilityManagementDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<EquipmentPlacementContractService> _logger;

    public EquipmentPlacementContractService(
        FacilityManagementDbContext context,
        IMapper mapper,
        ILogger<EquipmentPlacementContractService> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<EquipmentPlacementContractDto> CreateAsync(CreateEquipmentPlacementContractDto createContractDto)
    {
        var productionFacility = await _context.ProductionFacilities
                                                .FirstOrDefaultAsync(f => f.Code == createContractDto.ProductionFacilityCode);

        if (productionFacility == null)
        {
            throw new EntityNotFoundException($"Production facility with code {createContractDto.ProductionFacilityCode} not found.");
        }

        var equipmentType = await _context.ProcessEquipmentTypes
                                            .FirstOrDefaultAsync(e => e.Code == createContractDto.ProcessEquipmentTypeCode);

        if (equipmentType == null)
        {
            throw new EntityNotFoundException($"Equipment type with code {createContractDto.ProcessEquipmentTypeCode} not found.");
        }

        var totalRequiredArea = equipmentType.Area * createContractDto.EquipmentUnits;

        var usedArea = await _context.EquipmentPlacementContracts
                                    .Where(c => c.ProductionFacility.Code == createContractDto.ProductionFacilityCode)
                                    .SumAsync(c => c.EquipmentUnits * c.ProcessEquipmentType.Area);

        var availableArea = productionFacility.StandardArea - usedArea;

        if (availableArea < totalRequiredArea)
        {
            throw new InvalidOperationException("Not enough free area in the production facility to place the requested equipment.");
        }

        var newContract = _mapper.Map<EquipmentPlacementContract>(createContractDto);

        _context.EquipmentPlacementContracts.Add(newContract);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Successfully created equipment placement contract: {@Contract}", newContract);

        var equipmentPlacementContractDto = _mapper.Map<EquipmentPlacementContractDto>(newContract);
        return equipmentPlacementContractDto;
    }

    public async Task<IEnumerable<EquipmentPlacementContractDto>> GetAllAsync()
    {
        var contracts = await _context.EquipmentPlacementContracts
                                        .AsNoTracking()
                                        .ProjectTo<EquipmentPlacementContractDto>(_mapper.ConfigurationProvider)
                                        .OrderBy(x => x.ProductionFacilityName)
                                        .ToListAsync();

        _logger.LogInformation("Successfully retrieved all ({TerminationReasonCount}) equipment placement contracts", contracts.Count);
        return contracts;
    }
}