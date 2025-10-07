using Microsoft.AspNetCore.Mvc;
using RestaurantManagementSystem.Services;
using RestaurantManagementSystem.Utils;

namespace RestaurantManagementSystem.Controllers
{
    [ApiController]
    public class UploadsController: ControllerBase
    {
        private readonly IFileUploadService _fileUploadService;

        public UploadsController(IFileUploadService fileUploadService)
        {
            _fileUploadService = fileUploadService;
        }

        [HttpPost(ApiRoutes.DELETE_IMAGE)]
        public async Task<ActionResult<ApiResponse<object>>> DeleteFile([FromRoute] int fileId)
        {
            await _fileUploadService.DeleteFileAsync(fileId);
            return Ok(new ApiResponse<object> { Message = "File deleted successfully." });
        }
    }
}
