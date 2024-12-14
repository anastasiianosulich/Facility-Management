using FacilityLeasing.Services;
using FacilityLeasing.Exceptions;
using Moq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AutoMapper;
using FacilityLeasing.Dtos;
using FacilityLeasing.Entities;
using FacilityLeasing.Data;

namespace FacilityLeasing.Tests;

public class EquipmentPlacementContractServiceTests
{
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<EquipmentPlacementContractService>> _mockLogger;
    private readonly DbContextOptions<FacilityManagementDbContext> _dbOptions;
    private readonly EquipmentPlacementContractService _contractService;

    public EquipmentPlacementContractServiceTests()
    {
        var databaseName = Guid.NewGuid().ToString();

        // Setup InMemory Database
        _dbOptions = new DbContextOptionsBuilder<FacilityManagementDbContext>()
                            .UseInMemoryDatabase(databaseName)  
                            .Options;

        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<EquipmentPlacementContractService>>();

        var context = new FacilityManagementDbContext(_dbOptions);
        _contractService = new EquipmentPlacementContractService(context, _mockMapper.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnContractDto_WhenCreatedSuccessfully()
    {
        // Arrange
        var facility = new ProductionFacility { Code = "FacilityCode", StandardArea = 100, Name = "Facility Name" };
        var equipmentType = new ProcessEquipmentType { Code = "EquipmentCode", Area = 10, Name = "Equipment Name" };

        var createContractDto = new CreateEquipmentPlacementContractDto
        {
            ProductionFacilityCode = facility.Code,
            ProcessEquipmentTypeCode = equipmentType.Code,
            EquipmentUnits = 10,
        };

        var contractDto = new EquipmentPlacementContractDto
        {
            EquipmentUnits = createContractDto.EquipmentUnits,
            ProcessEquipmentTypeName = equipmentType.Name,
            ProductionFacilityName = facility.Name,
        };

        var contract = new EquipmentPlacementContract
        {
            EquipmentUnits = createContractDto.EquipmentUnits,
            ProcessEquipmentTypeCode = equipmentType.Code,
            ProductionFacilityCode = facility.Code,
        };

        using (var context = new FacilityManagementDbContext(_dbOptions))
        {
            context.ProductionFacilities.Add(facility);
            context.ProcessEquipmentTypes.Add(equipmentType);
            await context.SaveChangesAsync();
        }

        _mockMapper.Setup(m => m.Map<EquipmentPlacementContract>(It.IsAny<CreateEquipmentPlacementContractDto>()))
             .Returns(contract);

        _mockMapper.Setup(m => m.Map<EquipmentPlacementContractDto>(It.IsAny<EquipmentPlacementContract>()))
                   .Returns(contractDto);

        // Act
        var result = await _contractService.CreateAsync(createContractDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(result.ProductionFacilityName, facility.Name);
        Assert.Equal(result.ProcessEquipmentTypeName, equipmentType.Name);
        Assert.Equal(result.EquipmentUnits, createContractDto.EquipmentUnits);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowInvalidOperationException_WhenNotEnoughAreaAvailable()
    {
        // Arrange
        var facility = new ProductionFacility { Code = "FacilityCode", StandardArea = 100, Name = "Facility Name" };
        var equipmentType = new ProcessEquipmentType { Code = "EquipmentCode", Area = 10, Name = "Equipment Name" };
        var contract = new EquipmentPlacementContract
        {
            EquipmentUnits = 10,
            ProcessEquipmentTypeCode = equipmentType.Code,
            ProductionFacilityCode = facility.Code,
        };

        var createContractDto = new CreateEquipmentPlacementContractDto
        {
            ProductionFacilityCode = facility.Code,
            ProcessEquipmentTypeCode = equipmentType.Code,
            EquipmentUnits = 10
        };

        using (var context = new FacilityManagementDbContext(_dbOptions))
        {
            context.ProductionFacilities.Add(facility);
            context.ProcessEquipmentTypes.Add(equipmentType);
            context.EquipmentPlacementContracts.Add(contract);
            await context.SaveChangesAsync();
        }

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _contractService.CreateAsync(createContractDto));
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowEntityNotFoundException_WhenProductionFacilityNotFound()
    {
        // Arrange
        var facility = new ProductionFacility { Code = "FacilityCode", StandardArea = 100, Name = "Facility Name" };
        var equipmentType = new ProcessEquipmentType { Code = "EquipmentCode", Area = 10, Name = "Equipment Name" };

        var createContractDto = new CreateEquipmentPlacementContractDto
        {
            ProductionFacilityCode = facility.Code,
            ProcessEquipmentTypeCode = equipmentType.Code,
            EquipmentUnits = 10
        };

        using (var context = new FacilityManagementDbContext(_dbOptions))
        {
            context.ProductionFacilities.Add(facility);
            await context.SaveChangesAsync();
        }

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => _contractService.CreateAsync(createContractDto));
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowEntityNotFoundException_WhenEquipmentTypeNotFound()
    {
        var facility = new ProductionFacility { Code = "FacilityCode", StandardArea = 100, Name = "Facility Name" };
        var equipmentType = new ProcessEquipmentType { Code = "EquipmentCode", Area = 10, Name = "Equipment Name" };

        // Arrange
        var createContractDto = new CreateEquipmentPlacementContractDto
        {
            ProductionFacilityCode = facility.Code,
            ProcessEquipmentTypeCode = equipmentType.Code,
            EquipmentUnits = 10
        };

        using (var context = new FacilityManagementDbContext(_dbOptions))
        {
            context.ProcessEquipmentTypes.Add(equipmentType);
            await context.SaveChangesAsync();
        }

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => _contractService.CreateAsync(createContractDto));
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllContractsInOrder()
    {
        // Arrange
        var facility1 = new ProductionFacility { Code = "Facility1", StandardArea = 100, Name = "Facility Name 1" };
        var facility2 = new ProductionFacility { Code = "Facility2", StandardArea = 200, Name = "Facility Name 2" };
        var equipmentType1 = new ProcessEquipmentType { Code = "EquipmentCode1", Area = 10, Name = "Equipment Name 1" };
        var equipmentType2 = new ProcessEquipmentType { Code = "EquipmentCode2", Area = 10, Name = "Equipment Name 2" };

        var contract1 = new EquipmentPlacementContract
        {
            Id = Guid.NewGuid(),
            EquipmentUnits = 10,
            ProductionFacilityCode = facility1.Code,
            ProcessEquipmentTypeCode = equipmentType1.Code,
        };
        var contract2 = new EquipmentPlacementContract
        {
            Id = Guid.NewGuid(),
            EquipmentUnits = 5,
            ProductionFacilityCode = facility2.Code,
            ProcessEquipmentTypeCode = equipmentType2.Code,
        };
        var contracts = new[] { contract2, contract1 };

        using (var context = new FacilityManagementDbContext(_dbOptions))
        {
            context.ProductionFacilities.AddRange(facility1, facility2);
            context.ProcessEquipmentTypes.AddRange(equipmentType1, equipmentType2);
            context.EquipmentPlacementContracts.AddRange(contracts);
            await context.SaveChangesAsync();
        }

        _mockMapper.Setup(m => m.ConfigurationProvider)
            .Returns(new MapperConfiguration(cfg => cfg.CreateMap<EquipmentPlacementContract, EquipmentPlacementContractDto>()));

        // Act
        var result = await _contractService.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());

        var firstContract = result.First();
        Assert.Equal(contract1.EquipmentUnits, firstContract.EquipmentUnits);
        Assert.Equal(facility1.Name, firstContract.ProductionFacilityName);

        var lastContract = result.Last();
        Assert.Equal(contract2.EquipmentUnits, lastContract.EquipmentUnits);
        Assert.Equal(facility2.Name, lastContract.ProductionFacilityName);
    }
}
