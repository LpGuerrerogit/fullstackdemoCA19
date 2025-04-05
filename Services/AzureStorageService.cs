using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using System.Text;
using System.Text.Json;

public class AzureStorageService : IAzureStorageService {
    private readonly BlobServiceClient _blobServiceClient;

    public AzureStorageService(IConfiguration config) {
        var keyVaultUrl = config["KeyVaultUrl"];

        if (string.IsNullOrWhiteSpace(keyVaultUrl))
            throw new InvalidOperationException("Missing configuration: 'KeyVaultUrl'.");

        var client = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential());

        var secret = client.GetSecret("StorageConnectionString");
        _blobServiceClient = new BlobServiceClient(secret.Value.Value);
    }

    public async Task InsertUserDataAsync(DatoUsuario data) {
        var container = _blobServiceClient.GetBlobContainerClient("datosusuario");
        await container.CreateIfNotExistsAsync();
        var blob = container.GetBlobClient($"{data.Nombre}-{DateTime.UtcNow.Ticks}.json");
        var json = JsonSerializer.Serialize(data);
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
        await blob.UploadAsync(stream);
    }
    public async Task<string> GetImageAsDataUrlAsync(string containerName, string fileName)
    {
        try
        {
            var blobClient = _blobServiceClient.GetBlobContainerClient(containerName).GetBlobClient(fileName);

            if (!await blobClient.ExistsAsync())
                throw new FileNotFoundException($"El archivo '{fileName}' no se encontró en el contenedor '{containerName}'.");

            var stream = new MemoryStream();
            await blobClient.DownloadToAsync(stream);
            stream.Position = 0;

            var mimeType = Path.GetExtension(fileName).ToLower() switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".webp" => "image/webp",
                _ => "application/octet-stream"
            };

            var base64 = Convert.ToBase64String(stream.ToArray());
            return $"data:{mimeType};base64,{base64}";
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error al obtener la imagen: {ex.Message}", ex);
        }
    }

    public string GenerateSasToken(string containerName, string fileName, TimeSpan duration) {
        var container = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = container.GetBlobClient(fileName);
        var sasBuilder = new BlobSasBuilder {
            BlobContainerName = containerName,
            BlobName = fileName,
            ExpiresOn = DateTimeOffset.UtcNow.Add(duration),
            Resource = "b"
        };
        sasBuilder.SetPermissions(BlobSasPermissions.Read);
        return blobClient.GenerateSasUri(sasBuilder).ToString();
    }
}