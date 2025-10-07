using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;

namespace RestaurantManagementSystem.Exception
{
    public class ApiExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ApiExceptionFilter> _logger;

        public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
        {
            _logger = logger;
        }

        // Handle the exception and create a response
        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            _logger.LogError(exception, "An unhandled exception occurred.");

            int statusCode;
            int code;
            string message;

            switch (exception)
            {
                case ArgumentNullException:
                    statusCode = (int)HttpStatusCode.BadRequest; // Bad Request
                    code = 1001;
                    message = exception.Message;
                    break;
                case InvalidOperationException:
                    statusCode = (int)HttpStatusCode.Conflict; // Conflict
                    code = 1002;
                    message = exception.Message;
                    break;
                case KeyNotFoundException:
                    statusCode = (int)HttpStatusCode.NotFound; // Not Found
                    code = 1004;
                    message = exception.Message;
                    break;
                case SecurityTokenException:
                    statusCode = (int)HttpStatusCode.Unauthorized; // Unauthorized
                    code = 1005;
                    message = "Invalid token.";
                    break; 
                default:
                    statusCode = (int)HttpStatusCode.InternalServerError; // Internal Server Error
                    code = 1003;
                    message = "An unexpected error occurred.";
                    break;
            }

            // Create the response
            var response = new ApiResponse<string>
            {
                Code = code,
                Message = message,
                Result = null
            };

            context.Result = new ObjectResult(response)
            {
                StatusCode = statusCode
            };

            context.ExceptionHandled = true; // Mark exception as handled
        }
    }
}