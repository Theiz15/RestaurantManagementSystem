using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagementSystem.DTOs.Requests;
using RestaurantManagementSystem.DTOs.Responses;
using RestaurantManagementSystem.Services;

namespace RestaurantManagementSystem.Controllers
{
    [ApiController]
    [Route("api/payment")]

    
    public class PaymentController : ControllerBase
    {
        private readonly IVnPayService _vnPayService;

        public PaymentController(IVnPayService vnPayService)
        {
            _vnPayService = vnPayService;
        }

        [HttpPost("create")]
        public async Task<ActionResult<ApiResponse<string>>> CreatePaymentUrlVnpay(PaymentInformationModel model)
        {
            var url = await _vnPayService.CreatePaymentUrl(model, HttpContext);

            var response = new ApiResponse<string>
            {
                Code = 1000,
                Message = "Successful!",
                Result = url
            };

            return Ok(response);
        }

        [HttpGet("call-back")]
        public async Task<ActionResult<ApiResponse<PaymentResponseModel>>> PaymentCallbackVnpay()
        {
            var result =await _vnPayService.PaymentExecute(Request.Query);

            var response = new ApiResponse<PaymentResponseModel>
            {
                Code = 1000,
                Message = "Successful!",
                Result = result
            };

            return Ok(response);
        }
    }
}