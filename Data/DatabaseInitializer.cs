using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Repositories;

namespace RestaurantManagementSystem.Data
{
    public class DatabaseInitializer
    {
        private readonly RoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public DatabaseInitializer(RoleRepository roleRepository, IUserRepository userRepository, IConfiguration configuration)
        {
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public void SeedAdmin(AppDbContext appContext)
        {
            if (!appContext.Users.Any(u => u.Role.Name == "Admin"))
            {
                var adminRole = _roleRepository.GetRoleByName("Admin").Result;
                var admin = new User
                {
                    Username = _configuration["DefaultAdmin:Username"],
                    Password = BCrypt.Net.BCrypt.HashPassword(_configuration["DefaultAdmin:Password"]),
                    FullName = "Admin User",
                    Email = _configuration["DefaultAdmin:Email"],
                    CreatedAt = DateTime.UtcNow,
                    Phone = "0999999999",
                    Status = Enums.UserStatus.ACTIVE,
                    Role = adminRole
                };

                appContext.Users.Add(admin);
                appContext.SaveChanges();

                Console.WriteLine("Admin user created with username 'admin' and password 'admin123'.");


                // Optionally add the admin user to the context and save changes
                // appContext.Users.Add(admin);
                // appContext.SaveChanges();
            }
            else
            {
                Console.WriteLine("Admin user already exists. No action taken.");
            }
        }
        
    }
}