public interface IAzureStorageService {
    Task InsertUserDataAsync(DatoUsuario data);
    Task<string> GetImageAsDataUrlAsync(string containerName, string fileName);
    string GenerateSasToken(string containerName, string fileName, TimeSpan duration);
}