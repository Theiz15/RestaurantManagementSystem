namespace RestaurantManagementSystem.Services.Storage
{
    public interface IFileStorageService
    {
        Task<string> SaveFileAsync(Stream fileStream, string fileName, string subfolder);
        Task DeleteFileAsync(string filePath);
    }
}
