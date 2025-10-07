using RestaurantManagementSystem.DTOs;
using RestaurantManagementSystem.DTOs.Requests;
using RestaurantManagementSystem.DTOs.Responses;
using RestaurantManagementSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Services
{
    public interface IUserService
    {
        Task<UserResponse> GetUserByUserId(int userId);
        Task<IEnumerable<UserResponse>> GetAllUsers();
        Task<UserResponse> CreateUser(UserCreateDTO request);
        Task<UserResponse> UpdateUser(int userId, UserUpdateDTO request);
        Task DeleteUser(int userId);
    }
}