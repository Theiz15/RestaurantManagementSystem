using RestaurantManagementSystem.Data;
using RestaurantManagementSystem.Models;

namespace RestaurantManagementSystem.Repositories
{
    public class FileUploadRepository: GenericRepository<FileUpload> , IFileUploadRepository
    {
        public FileUploadRepository(AppDbContext context) : base(context)
        {
        }

    }
}
