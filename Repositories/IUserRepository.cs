using RestaurantManagementSystem.Models;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetByUsernameAsync(string username);
        Task<bool> existsByUsername(string username);
        Task UpdateAsync(User user);
        Task DeleteAsync(User user);
    }
}