using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Esf;
using RestaurantManagementSystem.DTOs.Requests;
using RestaurantManagementSystem.DTOs.Responses;
using RestaurantManagementSystem.Services;
using RestaurantManagementSystem.Utils;

namespace RestaurantManagementSystem.Controllers
{
    [ApiController]
    public class ShiftAssignmentController : ControllerBase
    {
        private readonly ShiftAssignmentService _shiftAssignmentService;

        public ShiftAssignmentController(ShiftAssignmentService shiftAssignmentService)
        {
            _shiftAssignmentService = shiftAssignmentService;
        }

        [HttpPost(ApiRoutes.CREATE_SHIFT_ASSIGNMENT)]
        public async Task<ActionResult<ApiResponse<ShiftAssignmentResponse>>> RegisterAssignment([FromBody] ShiftAssignmentRequest request)
        {
            var assignment = await _shiftAssignmentService.RegisterShift(request);

            var response = new ApiResponse<ShiftAssignmentResponse>
            {
                Code = 1000,
                Message = "Assignment shift successfully",
                Result = assignment
            };

            return Ok(response);
        }

        [HttpPost(ApiRoutes.CHECK_IN)]
        public async Task<ActionResult<ApiResponse<ShiftAssignmentResponse>>> CheckIn([FromRoute] int assignmentId)
        {
            Console.Write("Id Controller", assignmentId);
            var assignment = await _shiftAssignmentService.CheckIn(assignmentId);

            var response = new ApiResponse<ShiftAssignmentResponse>
            {
                Code = 1000,
                Message = "Check in assignment successfully",
                Result = assignment
            };

            return Ok(response);
        }

        [HttpPost(ApiRoutes.CHECK_OUT)]
        public async Task<ActionResult<ApiResponse<ShiftAssignmentResponse>>> CheckOut([FromRoute] int assignmentId)
        {
            var assignment = await _shiftAssignmentService.CheckOut(assignmentId);

            var response = new ApiResponse<ShiftAssignmentResponse>
            {
                Code = 1000,
                Message = "Check out assignment successfully!!",
                Result = assignment
            };

            return Ok(response);
        }

        [HttpGet(ApiRoutes.GET_ALL_SHIFT_ASSIGNMENT_BY_USER)]
        public async Task<ActionResult<ApiResponse<IEnumerable<ShiftAssignmentResponse>>>> GetALLShiftAssignmentByUser([FromRoute] int userId)
        {
            var result = await _shiftAssignmentService.GetAllShiftAssignment(userId);

            var response = new ApiResponse<IEnumerable<ShiftAssignmentResponse>>
            {
                Code = 1000,
                Message = "Get all shift assignment by user",
                Result = result
            };

            return Ok(response);
        }

        [HttpGet(ApiRoutes.GET_ALL_SHIFT_ASSIGNMENT)]
        public async Task<ActionResult<ApiResponse<IEnumerable<ShiftAssignmentResponse>>>> GetAllShiftAssignment()
        {
            var result = await _shiftAssignmentService.GetAllShiftAssignment();

            var response = new ApiResponse<IEnumerable<ShiftAssignmentResponse>>
            {
                Code = 1000,
                Message = "Get all shift assignment!!!",
                Result = result
            };

            return Ok(response);
        }

        [HttpPost(ApiRoutes.FILTER_SHIFT_ASSIGNMENT)]
        public async Task<ActionResult<ApiResponse<IEnumerable<ShiftAssignmentResponse>>>> FilterShiftAssignment([FromBody] FilterShiftAssignmentRequest request)
        {
            var result = await _shiftAssignmentService.FilterShiftAssignment(request);

            var response = new ApiResponse<IEnumerable<ShiftAssignmentResponse>>
            {
                Code = 1000,
                Message = "Successful!",
                Result = result
            };

            return Ok(response);
        }

        [HttpGet(ApiRoutes.GET_SHIFT_ASSIGNMENT)]
        public async Task<ActionResult<ApiResponse<ShiftAssignmentResponse>>> GetShiftAssignment([FromRoute] int userId)
        {
            var result = await _shiftAssignmentService.GetShiftAssignment(userId, DateTime.Today);

            var response = new ApiResponse<ShiftAssignmentResponse>
            {
                Code = 1000,
                Message = "Get shift assignment successful!!!",
                Result = result
            };

            return Ok(response);
        }

        [HttpDelete(ApiRoutes.CANCELED_SHIFT_ASSIGNMENT)]
        public async Task<ActionResult<ApiResponse<string>>> CanceledShiftAssignment ([FromRoute] int userId, [FromRoute] int assignmentId)
        {
            await _shiftAssignmentService.Canceled(userId, assignmentId);

            var response = new ApiResponse<string>
            {
                Code = 1000,
                Message = "Canceled successfully!!!",
                Result = null
            };

            return Ok(response);
        }
    }
}