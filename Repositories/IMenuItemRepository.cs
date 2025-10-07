using RestaurantManagementSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Repositories
{
    public interface IMenuItemRepository : IGenericRepository<MenuItem>
    {
        Task<IEnumerable<MenuItem>> GetAllWithCategoryAsync();
        Task<MenuItem?> GetByIdWithCategoryAsync(int id);
        Task<MenuItem?> GetByNameAsync(string name);
    }
}