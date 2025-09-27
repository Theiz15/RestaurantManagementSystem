using RestaurantManagementSystem.DTOs;
using RestaurantManagementSystem.DTOs.Requests;
using RestaurantManagementSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<User> CreateUserAsync(UserCreateDTO userDto);
        Task<bool> UpdateUserAsync(int id, UserUpdateDTO userDto);
        Task<bool> DeleteUserAsync(int id);
        // Add methods for authentication/authorization later
    }
}