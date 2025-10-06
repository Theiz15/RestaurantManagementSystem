using AutoMapper; // We will add AutoMapper later
using MailKit;
using Microsoft.AspNetCore.Identity;
using RestaurantManagementSystem.DTOs;
using RestaurantManagementSystem.DTOs.Requests;
using RestaurantManagementSystem.DTOs.Responses;
using RestaurantManagementSystem.Enums;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper; // For mapping between Model and DTO

        private readonly MailService _mailService;
        public UserService(IUserRepository userRepository, IMapper mapper, MailService mailService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _mailService = mailService;
        }

        public async Task<UserResponse> CreateUser(UserCreateDTO request)
        {
            if (string.IsNullOrEmpty(request.Username))
            {
                throw new ArgumentNullException(nameof(request.Username), "Username cannot be null or empty.");
            }

            if (await _userRepository.existsByUsername(request.Username))
            {
                // Handle the case where the username already exists
                throw new InvalidOperationException("Username already exists.");
            }

            var user = _mapper.Map<User>(request);

            // Hash the password before saving
            string passwordHasher = BCrypt.Net.BCrypt.HashPassword(request.Password);
            user.Password = passwordHasher;

            // Set default values
            user.Status = UserStatus.ACTIVE;
            user.CreatedAt = DateTime.UtcNow;

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            // Send welcome email
            await _mailService.SendEmailAsync(user.Email, "Welcome to Our Restaurant Management System", 
                $"Hello {user.FullName},\n\nWelcome to our Restaurant Management System! We're glad to have you on board!!!");
            return _mapper.Map<UserResponse>(user);
        }

        public async Task<IEnumerable<UserResponse>> GetAllUsers()
        {
            var users = await _userRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<UserResponse>>(users);
        }

        public async Task<UserResponse> GetUserByUserId(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            return _mapper.Map<UserResponse>(user);
        }

        public async Task<UserResponse> UpdateUser(int userId, UserUpdateDTO request)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            // Update fields
            _mapper.Map(request, user);
            user.UpdatedAt = DateTime.UtcNow;
            if (!string.IsNullOrEmpty(request.Password))
            {
                // Hash the new password before saving
                string passwordHasher = BCrypt.Net.BCrypt.HashPassword(request.Password);
                user.Password = passwordHasher;
            }
            await _userRepository.SaveChangesAsync();

            return _mapper.Map<UserResponse>(user);
        }

        public async Task DeleteUser(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            user.Status = UserStatus.INACTIVE; 
            await _userRepository.SaveChangesAsync();
        }

    }
}