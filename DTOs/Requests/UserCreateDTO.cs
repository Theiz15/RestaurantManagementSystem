using System.ComponentModel.DataAnnotations;

namespace RestaurantManagementSystem.DTOs
{
    public class UserCreateDTO
    {
        [Required(ErrorMessage = "Username is required.")]
        [MaxLength(100)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        [MaxLength(255)]
        public string Password { get; set; }

        [MaxLength(255)]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; }

        [Phone]
        [MaxLength(20)]
        public string Phone { get; set; }

        [Required]
        public int RoleId { get; set; }
    }
}