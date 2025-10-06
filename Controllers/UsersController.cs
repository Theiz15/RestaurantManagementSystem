using AutoMapper; // Cần IMapper trong Controller
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagementSystem.DTOs;
using RestaurantManagementSystem.DTOs.Requests;
using RestaurantManagementSystem.DTOs.Responses;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Services;
using RestaurantManagementSystem.Utils;

namespace RestaurantManagementSystem.Controllers
{
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UsersController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        // CREATE USER
        [HttpPost(ApiRoutes.CREATE_USER)]
        public async Task<ActionResult<ApiResponse<UserResponse>>> createUser([FromBody] UserCreateDTO request)
        {
            var createdUser = await _userService.CreateUser(request);
            var response = new ApiResponse<UserResponse>
            {
                Code = 1000,
                Message = "User created successfully",
                Result = createdUser
            };
            return Ok(response);
        }

        // GET USER BY USER_ID
        [HttpGet(ApiRoutes.GET_USER)]
        public async Task<ActionResult<ApiResponse<UserResponse>>> getUserById([FromRoute] int id)
        {
            var user = await _userService.GetUserByUserId(id);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }
            var response = new ApiResponse<UserResponse>
            {
                Code = 1000,
                Message = "User retrieved successfully.",
                Result = _mapper.Map<UserResponse>(user)
            };
            return Ok(response);

        }

        // GET ALL USERS
        [Authorize(Roles = "Admin")]
        [HttpGet(ApiRoutes.GET_USERS)]
        public async Task<ActionResult<ApiResponse<IEnumerable<UserResponse>>>> getAllUsers()
        {
            var users = await _userService.GetAllUsers();
            var response = new ApiResponse<IEnumerable<UserResponse>>
            {
                Code = 1000,
                Message = "Users retrieved successfully.",
                Result = users
            };
            return Ok(response);
        }

        //UPDATE USER
        [HttpPut(ApiRoutes.UPDATE_USER)]
        public async Task<ActionResult<ApiResponse<UserResponse>>> updateUser([FromRoute] int id, [FromBody] UserUpdateDTO request)
        {
            var updatedUser = await _userService.UpdateUser(id, request);
            var response = new ApiResponse<UserResponse>
            {
                Code = 1000,
                Message = "User updated successfully",
                Result = updatedUser
            };
            return Ok(response);
        }

        // DELETE USER
        [HttpDelete(ApiRoutes.DELETE_USER)]
        public async Task<ActionResult<ApiResponse<string>>> DeleteUser([FromRoute] int id)
        {
            await _userService.DeleteUser(id);
            var response = new ApiResponse<string>
            {
                Code = 1000,
                Message = "User deleted successfully",
                Result = null
            };
            return Ok(response);
        }
    }
}