using Azure.Storage.Blobs;
using FacilityLeasing.Common.Interfaces;

namespace FacilityLeasing.Common.Services;

public class AzureStorageService : IAzureStorageService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly string _containerName = "contracts";

    public AzureStorageService(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }

    public async Task<string> UploadBlobAsync(string blobName, byte[] content)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);

        await containerClient.CreateIfNotExistsAsync();

        var blobClient = containerClient.GetBlobClient(blobName);

        using var memoryStream = new MemoryStream(content);
        await blobClient.UploadAsync(memoryStream, overwrite: true);

        return blobClient.Uri.ToString();
    }
}
