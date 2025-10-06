using System.ComponentModel.DataAnnotations;
using RestaurantManagementSystem.Enums;

namespace RestaurantManagementSystem.DTOs
{
    public class UserUpdateDTO
    {

        [MaxLength(255)]
        public string? FullName { get; set; }

        [MaxLength(255)]
        public string? Password { get; set; }

        [EmailAddress]
        [MaxLength(255)]
        public string? Email { get; set; }

        [Phone]
        [MaxLength(20)]
        public string? Phone { get; set; }

        public UserStatus? Status { get; set; }

        // Can change role when updating user
        public int? RoleId { get; set; }
    }
}