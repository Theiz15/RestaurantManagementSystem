using RestaurantManagementSystem.DTOs.Responses;
using RestaurantManagementSystem.Enums;

namespace RestaurantManagementSystem.Services
{
    public interface IFileUploadService
    {
        Task<IEnumerable<FileUploadResponse>> UploadImagesForMenuItemAsync(List<IFormFile> files, int menuItemId, FileType type);

        Task<FileUploadResponse> AssignImageToCategoryAsync(IFormFile file, int categoryId);

        Task DeleteFileAsync(int fileId);
    }
}
