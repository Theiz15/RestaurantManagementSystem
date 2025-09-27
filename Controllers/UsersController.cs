using AutoMapper; // Cần IMapper trong Controller
using Microsoft.AspNetCore.Mvc;
using RestaurantManagementSystem.DTOs;
using RestaurantManagementSystem.DTOs.Requests;
using RestaurantManagementSystem.DTOs.Responses;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Services;

namespace RestaurantManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper; // Tiêm IMapper vào

        public UsersController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetUsers()
        {
            var userModels = await _userService.GetAllUsersAsync();  
            var userResponses = _mapper.Map<IEnumerable<UserResponse>>(userModels);
            return Ok(userResponses);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponse>> GetUser(int id)
        {
            var userModel = await _userService.GetUserByIdAsync(id);
            if (userModel == null) return NotFound();

            var userResponse = _mapper.Map<UserResponse>(userModel);
            return Ok(userResponse);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<User>>> CreateUser([FromBody] UserCreateDTO userDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            ApiResponse<User> apiResponse = new ApiResponse<User>()
            {
                Code = 1000,
                Message = "user",
                Result = await _userService.CreateUserAsync(userDto)

            };


            return apiResponse;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateDTO userDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _userService.UpdateUserAsync(id, userDto);
            if (!result) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}