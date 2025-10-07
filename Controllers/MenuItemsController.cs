using Microsoft.AspNetCore.Mvc;
using RestaurantManagementSystem.DTOs;
using RestaurantManagementSystem.DTOs.Requests;
using RestaurantManagementSystem.DTOs.Responses;
using RestaurantManagementSystem.Enums;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Services;
using RestaurantManagementSystem.Utils;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Controllers
{
    [ApiController]
    public class MenuItemsController : ControllerBase
    {
        private readonly IMenuItemService _menuItemService;
        private readonly IFileUploadService _fileUploadService;

        public MenuItemsController(IMenuItemService menuItemService, IFileUploadService fileUploadService)
        {
            _menuItemService = menuItemService;
            _fileUploadService = fileUploadService;
        }

        [HttpGet(ApiRoutes.GET_MENU_ITEMS)]
        public async Task<ActionResult<ApiResponse<IEnumerable<MenuItemResponse>>>> GetMenuItems()
        {
            var menuItemResponses = await _menuItemService.GetAllMenuItemsAsync();
            var response = new ApiResponse<IEnumerable<MenuItemResponse>>
            {
                Message = "Menu items retrieved successfully.",
                Result = menuItemResponses
            };
            return Ok(response);
        }

        [HttpGet(ApiRoutes.GET_MENU_ITEM)]
        public async Task<ActionResult<ApiResponse<MenuItemResponse>>> GetMenuItem([FromRoute] int id)
        {
            // Service trả về thẳng response object
            var menuItemResponse = await _menuItemService.GetMenuItemByIdAsync(id);
            var response = new ApiResponse<MenuItemResponse>
            {
                Message = $"Menu item with ID {id} retrieved successfully.",
                Result = menuItemResponse
            };
            return Ok(response);
        }

        [HttpPost(ApiRoutes.CREATE_MENU_ITEM)]
        public async Task<ActionResult<ApiResponse<MenuItemResponse>>> CreateMenuItem([FromBody] MenuItemCreateDTO menuItemDto)
        {
            var newMenuItemResponse = await _menuItemService.CreateMenuItemAsync(menuItemDto);
            var response = new ApiResponse<MenuItemResponse>
            {
                Message = "Menu item created successfully.",
                Result = newMenuItemResponse
            };
            return CreatedAtAction(nameof(GetMenuItem), new { id = newMenuItemResponse.Id }, response);
        }

        [HttpPut(ApiRoutes.UPDATE_MENU_ITEM)]
        public async Task<ActionResult<ApiResponse<object>>> UpdateMenuItem([FromRoute] int id, [FromBody] MenuItemUpdateDTO menuItemDto)
        {
            var response = new ApiResponse<object>
            {
                Message = $"Menu item with ID {id} updated successfully.",
                Result = await _menuItemService.UpdateMenuItemAsync(id, menuItemDto)
            };
            return Ok(response);
        }

        [HttpDelete(ApiRoutes.DELETE_MENU_ITEM)]
        public async Task<ActionResult<ApiResponse<object>>> DeleteMenuItem([FromRoute] int id)
        {
            await _menuItemService.DeleteMenuItemAsync(id);
            var response = new ApiResponse<object>
            {
                Message = $"Menu item with ID {id} deleted successfully.",
                Result = null
            };
            return Ok(response);
        }

        [HttpPost(ApiRoutes.UPLOAD_MENU_ITEM_IMAGES)]
        //[Consumes("multipart/form-data")]
        public async Task<ActionResult<ApiResponse<FileUploadResponse>>> UploadImageForMenuItem([FromRoute] int id,
                    [FromForm] List<IFormFile> files,
                    [FromForm] FileType type = FileType.GALLERY)
        {
            var fileResponses = await _fileUploadService.UploadImagesForMenuItemAsync(files, id, type);

            var response = new ApiResponse<IEnumerable<FileUploadResponse>>
            {
                Message = $"{files.Count} image(s) uploaded for menu item successfully.",
                Result = fileResponses
            };

            return Ok(response);
        }
    }

}
