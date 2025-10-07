using AutoMapper;
using RestaurantManagementSystem.Data;
using RestaurantManagementSystem.DTOs.Responses;
using RestaurantManagementSystem.Enums;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Repositories;
using RestaurantManagementSystem.Services.Storage;

namespace RestaurantManagementSystem.Services.Impl
{
    public class FileUploadServiceImpl : IFileUploadService
    {
        private readonly IFileUploadRepository _fileUploadRepository;
        private readonly IFileStorageService _fileStorageService;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMenuItemRepository _menuItemRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public FileUploadServiceImpl(
            IFileUploadRepository fileUploadRepository,
            IFileStorageService fileStorageService,
            ICategoryRepository categoryRepository,
            IMenuItemRepository menuItemRepository,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            IMapper mapper)
        {
            _fileUploadRepository = fileUploadRepository;
            _fileStorageService = fileStorageService;
            _categoryRepository = categoryRepository;
            _menuItemRepository = menuItemRepository;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _mapper = mapper;
        }
        public async Task<IEnumerable<FileUploadResponse>> UploadImagesForMenuItemAsync(List<IFormFile> files, int menuItemId, FileType type)
        {
            if (files == null || !files.Any())
            {
                throw new ArgumentException("At least one file is required.");
            }

            var menuItem = await _menuItemRepository.GetByIdAsync(menuItemId);
            if (menuItem == null)
            {
                throw new KeyNotFoundException($"Menu item with ID {menuItemId} not found.");
            }

            var createdFiles = new List<FileUpload>();

            // Lặp qua từng file trong danh sách
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    ValidateFile(file);

                    await using var stream = file.OpenReadStream();
                    var relativePath = await _fileStorageService.SaveFileAsync(stream, file.FileName, "menu_items");

                    var fileUpload = new FileUpload
                    {
                        Name = file.FileName,
                        Path = relativePath,
                        Location = "Local",
                        FileType = type,
                        MenuItemId = menuItemId
                    };
                    createdFiles.Add(fileUpload);
                }
            }

            if (!createdFiles.Any())
            {
                throw new ArgumentException("No valid files were provided.");
            }

            // Thêm tất cả các file vào context một lần
            await _fileUploadRepository.AddRangeAsync(createdFiles);
            // Lưu tất cả thay đổi vào DB chỉ với một lệnh gọi
            await _fileUploadRepository.SaveChangesAsync();

            return createdFiles.Select(MapToFileUploadResponse).ToList();
        }


        public async Task<FileUploadResponse> AssignImageToCategoryAsync(IFormFile file, int categoryId)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File is null or empty");
            }

            var category = await _categoryRepository.GetByIdAsync(categoryId);
            if (category == null)
            {
                throw new ArgumentException("Category not found");
            }

            ValidateFile(file);

            // Xóa ảnh cũ nếu có 
            if (category.FileUploadId.HasValue)
            {
                await DeleteFileAsync(category.FileUploadId.Value);
            }
            await using var stream = file.OpenReadStream();

            var relativeFilePath = await _fileStorageService.SaveFileAsync(stream, file.FileName, "categories");
            var fileUpload = new FileUpload
            {
                Name = file.FileName,
                Path = relativeFilePath,
                Location = "Local",
                FileType = FileType.AVATAR,
                MenuItemId = null
            };
            await _fileUploadRepository.AddAsync(fileUpload);
            await _fileUploadRepository.SaveChangesAsync();

            // Cập nhật lại Category với FileUploadId mới
            category.FileUploadId = fileUpload.Id;
            _categoryRepository.Update(category);
            await _categoryRepository.SaveChangesAsync();


            return _mapper.Map<FileUploadResponse>(fileUpload);
        }

        public async Task DeleteFileAsync(int fileId)
        {
            var fileUpload = await _fileUploadRepository.GetByIdAsync(fileId);
            if (fileUpload == null)
            {
                throw new ArgumentException("File not found");
            }

            //Nếu là ảnh của category , cập nhật category để xóa tham chiếu
            var linkedCategory = await _categoryRepository.SingleOrDefaultAsync(c => c.FileUploadId == fileId);
            if (linkedCategory != null)
            {
                linkedCategory.FileUploadId = null;
                _categoryRepository.Update(linkedCategory);
            }

            //Xóa file vât lý
            await _fileStorageService.DeleteFileAsync(fileUpload.Path);

            //Xóa bản ghi trong database
            _fileUploadRepository.Delete(fileUpload);
            await _fileUploadRepository.SaveChangesAsync();
        }

        private FileUploadResponse MapToFileUploadResponse(FileUpload fileUpload)
        {
            var response = _mapper.Map<FileUploadResponse>(fileUpload);
            var request = _httpContextAccessor.HttpContext!.Request;
            var requestPath = _configuration.GetValue<string>("StorageSettings:RequestPath") ?? "/static";
            response.Url = $"{request.Scheme}://{request.Host}{requestPath}/{fileUpload.Path}";
            return response; ;
        }

        private void ValidateFile(IFormFile file)
        {
            // 1. Kiểm tra kích thước file (tối đa 10MB)
            const int maxFileSizeInBytes = 10 * 1024 * 1024; // 10 MB
            if (file.Length > maxFileSizeInBytes)
            {
                // Ném lỗi này sẽ được ApiExceptionFilter chuyển thành 400 Bad Request
                throw new ArgumentException($"File size cannot exceed 10MB. File '{file.FileName}' is too large.");
            }

            // 2. Kiểm tra loại file (chỉ cho phép ảnh)
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(fileExtension) || !allowedExtensions.Contains(fileExtension))
            {
                throw new ArgumentException($"Invalid file type. Only {string.Join(", ", allowedExtensions)} are allowed.");
            }
        }
    }
}
