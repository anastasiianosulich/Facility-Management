using System.Threading.Channels;
using FacilityLeasing.Common.Interfaces;
using FacilityLeasing.Dtos;

public class PdfBackgroundService : BackgroundService
{
    private readonly Channel<EquipmentPlacementContractDto> _contractsQueue;  
    private readonly ILogger<PdfBackgroundService> _logger;
    private readonly IPdfGenerator _pdfGenerator;
    private readonly IAzureStorageService _azureStorageService;

    public PdfBackgroundService(
        ILogger<PdfBackgroundService> logger,
        IPdfGenerator pdfGenerator,
        IAzureStorageService azureStorageService)
    {
        _contractsQueue = Channel.CreateUnbounded<EquipmentPlacementContractDto>();  
        _logger = logger;
        _pdfGenerator = pdfGenerator;
        _azureStorageService = azureStorageService;
    }

    public void EnqueueNewContract(EquipmentPlacementContractDto contract)
    {
        _contractsQueue.Writer.TryWrite(contract);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var contract in _contractsQueue.Reader.ReadAllAsync(stoppingToken))
        {
            try
            {
                var pdfContent = _pdfGenerator.GenerateContractPdf(contract);
                var blobName = $"contract-{contract.Id}.pdf"; 
                var blobUrl = await _azureStorageService.UploadBlobAsync(blobName, pdfContent);

                _logger.LogInformation($"Successfully generated and uploaded PDF for contract {contract.Id} to {blobUrl}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing background task for contract PDF generation.");
            }
        }
    }
}
