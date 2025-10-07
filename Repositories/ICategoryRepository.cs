using RestaurantManagementSystem.Models;

namespace RestaurantManagementSystem.Repositories
{
    public interface ICategoryRepository: IGenericRepository<Category>
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category> GetByIdAsync(int id);
        Task AddAsync(Category category);
        void Update(Category category);
        void Delete(Category category);
        Task<bool> SaveChangesAsync();
        Task<Category> GetByNameAsync(string? name);
    }
}
