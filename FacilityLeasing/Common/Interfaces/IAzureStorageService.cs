namespace FacilityLeasing.Common.Interfaces;

public interface IAzureStorageService
{
    Task<string> UploadBlobAsync(string blobName, byte[] content);
}
