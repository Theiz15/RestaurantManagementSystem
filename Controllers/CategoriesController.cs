using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagementSystem.DTOs.Requests;
using RestaurantManagementSystem.DTOs.Responses;
using RestaurantManagementSystem.Services;
using RestaurantManagementSystem.Utils;

namespace RestaurantManagementSystem.Controllers
{
    [ApiController]

    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _service;
        private readonly IMapper _mapper;
        private readonly IFileUploadService _fileUploadService;

        public CategoriesController(ICategoryService service, IMapper mapper, IFileUploadService fileUploadService)
        {
            _service = service;
            _mapper = mapper;
            _fileUploadService = fileUploadService;
        }

        [HttpGet(ApiRoutes.GET_CATEGORIES)]
        public async Task<ActionResult<ApiResponse<CategoryResponse>>> GetCategories()
        {
            var categories = await _service.GetCategories();
            var response = new ApiResponse<IEnumerable<CategoryResponse>>
            {
                Code = 1000,
                Message = "Categories retrieved successfully.",
                Result = categories
            };
            return Ok(response);
        }

        [HttpGet(ApiRoutes.GET_CATEGORY)]
        public async Task<ActionResult<ApiResponse<CategoryResponse>>> GetById([FromRoute] int id)
        {
            var categoryResponse = await _service.GetByIdAsync(id);
            var response = new ApiResponse<CategoryResponse>
            {
                Code = 1000,
                Message = "Category retrieved successfully.",
                Result = categoryResponse
            };
            return Ok(response);
        }

        [HttpPost(ApiRoutes.CREATE_CATEGORY)]
        public async Task<ActionResult<ApiResponse<CategoryResponse>>> CreateCategory([FromBody] CreateCategoryDTO dto)
        {
            var createdCategoryResponse = await _service.CreateCategoryAsync(dto);
            var response = new ApiResponse<CategoryResponse>
            {
                Code = 1000,
                Message = "Category created successfully.",
                Result = createdCategoryResponse
            };
            return Ok(response);
        }

        [HttpPut(ApiRoutes.UPDATE_CATEGORY)]
        public async Task<ActionResult<ApiResponse<CategoryResponse>>> Update([FromRoute] int id, [FromBody] CreateCategoryDTO dto)
        {
            var updatedCategory = await _service.UpdateAsync(id, dto);
            var response = new ApiResponse<CategoryResponse>
            {
                Code = 1000,
                Message = "Update successfuly",
                Result = updatedCategory
            };

            return Ok(response);

        }

        [HttpDelete(ApiRoutes.DELETE_CATEGORY)]
        public async Task<IActionResult> Delete(int id)
        {
           await _service.DeleteAsync(id);
           
            var response = new ApiResponse<string>
            {
                Code = 1000,
                Message = "Delete successfuly",
                Result = null
            };

            return Ok(response);
        }

        [HttpPost(ApiRoutes.UPLOAD_CATEGORY_IMAGES)]
        public async Task<ActionResult<ApiResponse<FileUploadResponse>>> UploadCategoryImage([FromRoute] int id,[FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new ApiResponse<string>
                {
                    Code = 1001,
                    Message = "No file uploaded.",
                    Result = null
                });
            }
            var fileUploadResponse = await _fileUploadService.AssignImageToCategoryAsync(file, id);
            var response = new ApiResponse<FileUploadResponse>
            {
                Code = 1000,
                Message = "File uploaded successfully.",
                Result = fileUploadResponse
            };
            return Ok(response);
        }

    }
}
