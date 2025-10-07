using RestaurantManagementSystem.DTOs.Requests;
using RestaurantManagementSystem.DTOs.Responses;

namespace RestaurantManagementSystem.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryResponse>> GetCategories();
        Task<CategoryResponse> GetByIdAsync(int id);
        Task<CategoryResponse> CreateCategoryAsync(CreateCategoryDTO dto);
        Task<CategoryResponse> UpdateAsync(int id, CreateCategoryDTO dto);
        Task DeleteAsync(int id);
    }
}
