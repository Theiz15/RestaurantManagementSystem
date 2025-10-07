
namespace RestaurantManagementSystem.Services.Storage
{
    public class LocalStorageService : IFileStorageService
    {
        private readonly string _storagePath;

        public LocalStorageService( IWebHostEnvironment webHostEnvironment)
        {
            var webRoot = webHostEnvironment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var storedFilesPath = Path.Combine(webRoot, "images");

            // Nếu thư mục wwwroot/images chưa tồn tại thì tạo mới
            if (!Directory.Exists(storedFilesPath))
            {
                Directory.CreateDirectory(storedFilesPath);
            }

            _storagePath = storedFilesPath;

        }

        public async Task<string> SaveFileAsync(Stream fileStream, string fileName, string subfolder)
        {
            //Tạo đường dẫn đầy đủ đến thư mục con
            var subfolderPath = Path.Combine(_storagePath, subfolder);
            if (!Directory.Exists(subfolderPath))
            {
                Directory.CreateDirectory(subfolderPath);
            }

            //Tạo tên file duy nhất để tránh trùng lặp
            var fileExtension = Path.GetExtension(fileName);
            var safeFileName = Path.GetFileNameWithoutExtension(fileName).Replace(" ","_");
            var uniqueFileName = $"{safeFileName}_{Guid.NewGuid()}{fileExtension}";
            var absoluteFilePath = Path.Combine(subfolderPath, uniqueFileName);

            using (var newFileStream = new FileStream(absoluteFilePath, FileMode.Create))
            {
                fileStream.CopyTo(newFileStream);
            }

            //Trả về đường dẫn tương đối để lưu vào cơ sở dữ liệu
            //var relativeFilePath = Path.Combine("images", subfolder, uniqueFileName).Replace("\\", "/");
            return Path.Combine(subfolder, uniqueFileName).Replace("\\", "/");
        }

        public Task DeleteFileAsync(string filePath)
        {
            if(string.IsNullOrEmpty(filePath))
            {
                return Task.CompletedTask;
            }
            var absoluteFilePath = Path.Combine(_storagePath, filePath);

            if(File.Exists(absoluteFilePath))
            {
                File.Delete(absoluteFilePath);
            }
            return Task.CompletedTask;
        }

        
    }
}
