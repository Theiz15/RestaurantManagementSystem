using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Google.Apis.Auth;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using RestaurantManagementSystem.DTOs.Requests;
using RestaurantManagementSystem.DTOs.Responses;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Repositories;

namespace RestaurantManagementSystem.Services
{
    public class AuthenticationService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly RoleRepository _roleRepository;
        private readonly InvalidTokenRepository _InvalidTokenRepository;
        private const int ExpireMinutes = 60;

        public AuthenticationService(IUserService userService, IConfiguration configuration, IUserRepository userRepository, RoleRepository roleRepository, InvalidTokenRepository invalidTokenRepository)
        {
            _userService = userService;
            _userRepository = userRepository;
            _configuration = configuration;
            _roleRepository = roleRepository;
            _InvalidTokenRepository = invalidTokenRepository;
        }

        // User login by username and password
        public async Task<AuthenticationResponse> login(AuthenticationRequest request)
        {
            User? user = await _userRepository.GetByUsernameAsync(request.Username);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                throw new UnauthorizedAccessException("Invalid password");
            }

            var token = GenerateJwtToken(user);

            return new AuthenticationResponse
            {
                successful = true,
                token = token,
            };
        }

        // User login by Google OAuth
        public async Task<AuthenticationResponse> googleLogin(GoogleLoginRequest request)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string>() { _configuration["Authentication:Google:ClientId"] }
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken, settings);

            // Check if user exists in the database
            var user = await _userRepository.GetUserByEmailAsync(payload.Email);

            if (user == null)
            {
                // If user doesn't exist, create a new one
                user = new User
                {
                    Email = payload.Email,
                    FullName = payload.Name,
                    // Other properties
                };
                await _userRepository.AddAsync(user);
            }

            var token = GenerateJwtToken(user);

            return new AuthenticationResponse
            {
                successful = true,
                token = token,
            };
        }

        // User login by Facebook OAuth
        public async Task<AuthenticationResponse> LoginWithFacebookAsync(string accessToken)
        {
            // Validate the access token with Facebook's API
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"https://graph.facebook.com/me?access_token={accessToken}&fields=id,name,email");
            if (!response.IsSuccessStatusCode)
            {
                throw new UnauthorizedAccessException("Invalid Facebook access token");
            }

            // Console.WriteLine("[FacebookLogin] Response:");
            // Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(response));

            var content = await response.Content.ReadAsStringAsync();
            var facebookUser = System.Text.Json.JsonSerializer.Deserialize<User>(content);

            // Check if user exists in the database
            var user = await _userRepository.GetUserByEmailAsync(facebookUser.Email);
            if (user == null)
            {
                // If user doesn't exist, create a new one
                user = new User
                {
                    Email = facebookUser.Email,
                    Username = facebookUser.Username == null ? facebookUser.Username : facebookUser.Email,
                    FullName = facebookUser.FullName == null ? facebookUser.Username : facebookUser.FullName,
                    // Other properties
                };
                await _userRepository.AddAsync(user);
            }

            var token = GenerateJwtToken(user);

            return new AuthenticationResponse
            {
                successful = true,
                token = token,
            };
        }

        // Logout user
        public async Task LogoutAsync(LogoutRequest request)
        {
            if (string.IsNullOrEmpty(request.Token))
            {
                throw new ArgumentException("Token is required for logout.");
            }

            var handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken;

            try
            {
                jwtToken = handler.ReadJwtToken(request.Token);
            }
            catch (System.Exception)
            {
                throw new ArgumentException("Invalid token format.");
            }

            var jti = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
            if (string.IsNullOrEmpty(jti))
            {
                throw new ArgumentException("Token does not contain a valid JTI.");
            }

            if (await _InvalidTokenRepository.ExistsAsync(jti))
            {
                throw new InvalidOperationException("Token has already been invalidated.");
            }

            var invalidToken = new Entities.InvalidToken
            {
                Id = jti,
                ExpiryDate = jwtToken.ValidTo
            };

            await _InvalidTokenRepository.AddAsync(invalidToken);
            await _InvalidTokenRepository.SaveChangesAsync();
        }

        // Verify token
        public async Task<IntrospectResponse> IsTokenValidAsync(IntrospectRequest request)
        {
            var handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken;

            try
            {
                jwtToken = handler.ReadJwtToken(request.Token);
            }
            catch (System.Exception)
            {
                return new IntrospectResponse { valid = false }; // Invalid token format
            }

            var jti = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
            if (string.IsNullOrEmpty(jti))
            {
                return new IntrospectResponse { valid = false }; // Token does not contain a valid JTI
            }

            bool isInvalidated = await _InvalidTokenRepository.ExistsAsync(jti);
            return new IntrospectResponse { valid = !isInvalidated }; // Token is valid if it has not been invalidated
        }
        
        // Generate JWT token
        private string GenerateJwtToken(User user)
        {
            var secretKey = _configuration["Jwt:Key"];
            var creds = new SigningCredentials(new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey)), SecurityAlgorithms.HmacSha512);

            var role = _roleRepository.GetByIdAsync(user.RoleId ?? 3).Result;
            var roleName = role.Name ?? "Staff";
            var claims = new[]
            {
                new Claim("Username", user.Username ?? user.Email ?? "unknown"),
                new Claim("Role", roleName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            Console.WriteLine("User Role: " + (user.RoleId ?? 0));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(ExpireMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}