using System.ComponentModel.DataAnnotations;

namespace RestaurantManagementSystem.DTOs.Requests
{
    public class MenuItemCreateDTO
    {
        [Required]
        [MaxLength(255)]
        public string ProductName { get; set; }
        public string? Description { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public bool IsAvailable { get; set; } = true;
    }
}
