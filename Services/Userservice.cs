using AutoMapper; // We will add AutoMapper later
using RestaurantManagementSystem.DTOs;
using RestaurantManagementSystem.DTOs.Requests;
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

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserCreateDTO>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UserCreateDTO>>(users);
        }

        public async Task<UserCreateDTO> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return _mapper.Map<UserCreateDTO>(user);
        }

        //public async Task<UserCreateDTO> CreateUserAsync(UserCreateDTO userDto)
        //{
        //    var user = _mapper.Map<User>(userDto);
        //    user.CreatedAt = System.DateTime.UtcNow;
        //    // Hash password here in a real app: user.Password = _passwordHasher.HashPassword(user.Password);

        //    await _userRepository.AddAsync(user);
        //    await _userRepository.SaveChangesAsync();
        //    return _mapper.Map<UserCreateDTO>(user);
        //}

        public async Task<bool> UpdateUserAsync(int id, UserCreateDTO userDto)
        {
            var existingUser = await _userRepository.GetByIdAsync(id);
            if (existingUser == null)
            {
                return false;
            }

            _mapper.Map(userDto, existingUser); // Map updated fields to existing entity
            existingUser.UpdatedAt = System.DateTime.UtcNow;

            _userRepository.Update(existingUser);
            await _userRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return false;
            }

            _userRepository.Delete(user);
            await _userRepository.SaveChangesAsync();
            return true;
        }

        Task<IEnumerable<User>> IUserService.GetAllUsersAsync()
        {
            throw new NotImplementedException();
        }

        Task<User> IUserService.GetUserByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<User> CreateUserAsync(UserCreateDTO userDto)
        {
            var user = _mapper.Map<User>(userDto);
            user.CreatedAt = System.DateTime.UtcNow;
            // Hash password here in a real app: user.Password = _passwordHasher.HashPassword(user.Password);

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();
            return user;
        }

        public Task<bool> UpdateUserAsync(int id, UserUpdateDTO userDto)
        {
            throw new NotImplementedException();
        }
    }
}