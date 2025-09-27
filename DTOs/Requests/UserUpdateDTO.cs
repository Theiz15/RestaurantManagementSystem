using System.ComponentModel.DataAnnotations;

namespace RestaurantManagementSystem.DTOs
{
    public class UserUpdateDTO
    {

        [MaxLength(255)]
        public string FullName { get; set; }

        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; }

        [Phone]
        [MaxLength(20)]
        public string Phone { get; set; }

        // Có thể cho phép thay đổi RoleId
        public int? RoleId { get; set; }
    }
}