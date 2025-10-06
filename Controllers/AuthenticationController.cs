using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestaurantManagementSystem.DTOs.Requests;
using RestaurantManagementSystem.DTOs.Responses;
using RestaurantManagementSystem.Services;
using RestaurantManagementSystem.Utils;

namespace RestaurantManagementSystem.Controllers
{
    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly AuthenticationService authenticationService;

        public AuthenticationController(ILogger<AuthenticationController> logger, AuthenticationService authenticationService)
        {
            _logger = logger;
            this.authenticationService = authenticationService;
        }

        // Login with username and password
        [HttpPost(ApiRoutes.LOGIN)]
        public async Task<ActionResult<ApiResponse<AuthenticationResponse>>> Login([FromBody] AuthenticationRequest request)
        {
            var authResponse = await authenticationService.login(request);

            var response = new ApiResponse<AuthenticationResponse>
            {
                Code = 1000,
                Message = "Login successful",
                Result = authResponse
            };

            return Ok(response);
        }

        //Login with Google Oauth
        [HttpPost(ApiRoutes.GOOGLE_LOGIN)]
        public async Task<ActionResult<ApiResponse<AuthenticationResponse>>> GoogleLogin([FromBody] GoogleLoginRequest request)
        {
            var authResponse = await authenticationService.googleLogin(request);

            var response = new ApiResponse<AuthenticationResponse>
            {
                Code = 1000,
                Message = "Login successful",
                Result = authResponse
            };

            return Ok(response);
        }

        //Login with Facebook Oauth
        [HttpPost(ApiRoutes.FACEBOOK_LOGIN)]
        public async Task<ActionResult<ApiResponse<AuthenticationResponse>>> FacebookLogin([FromBody] FaceBookLoginRequest request)
        {
            var authResponse = await authenticationService.LoginWithFacebookAsync(request.AccessToken);

            var response = new ApiResponse<AuthenticationResponse>
            {
                Code = 1000,
                Message = "Login successful",
                Result = authResponse
            };

            return Ok(response);
        }

        // Logout
        [HttpPost(ApiRoutes.LOGOUT)]
        public async Task<ActionResult<ApiResponse<string>>> Logout([FromBody] LogoutRequest request)
        {
            await authenticationService.LogoutAsync(request);

            var response = new ApiResponse<string>
            {
                Code = 1000,
                Message = "Logout successful",
                Result = null
            };

            return Ok(response);
        }

        // Verify token
        [HttpPost(ApiRoutes.INTROSPECT)]
        public async Task<ActionResult<ApiResponse<IntrospectResponse>>> IsTokenValid([FromBody] IntrospectRequest request)
        {
            var isValid = await authenticationService.IsTokenValidAsync(request);

            var response = new ApiResponse<IntrospectResponse>
            {
                Code = 1000,
                Message = "Token is valid",
                Result = isValid
            };

            return Ok(response);
        }
    }
}