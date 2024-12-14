using Microsoft.AspNetCore.Mvc;
using FacilityLeasing.Dtos;
using FacilityLeasing.Interfaces;

namespace FacilityLeasing.Controllers;

[ApiController]
[Route("api/equipment-placement-contracts")]
public class EquipmentPlacementContractController : ControllerBase
{
    private readonly IEquipmentPlacementContractService _contractService;
    private readonly PdfBackgroundService _pdfBackgroundService;

    public EquipmentPlacementContractController(
        IEquipmentPlacementContractService contractService,
        PdfBackgroundService pdfBackgroundService)
    {
        _contractService = contractService;
        _pdfBackgroundService = pdfBackgroundService;
    }

    // GET: api/equipment-placement-contracts
    [HttpGet]
    public async Task<IActionResult> GetEquipmentPlacementContracts()
    {
        var contracts = await _contractService.GetAllAsync();
        return Ok(contracts);
    }

    // POST: api/equipment-placement-contracts
    [HttpPost]
    public async Task<IActionResult> CreateEquipmentPlacementContract([FromBody] CreateEquipmentPlacementContractDto createContractDto)
    {
        var contract = await _contractService.CreateAsync(createContractDto);

        _pdfBackgroundService.EnqueueNewContract(contract);

        return CreatedAtAction(nameof(GetEquipmentPlacementContracts), new { id = contract.Id }, contract);
    }
}
