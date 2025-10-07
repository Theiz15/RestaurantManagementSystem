using System.ComponentModel.DataAnnotations;

namespace RestaurantManagementSystem.DTOs.Requests
{
    public class MenuItemUpdateDTO
    {
        [MaxLength(255)]
        public string? ProductName { get; set; }

        public string? Description { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? Price { get; set; }

        public int? CategoryId { get; set; }

        public bool? IsAvailable { get; set; }
    }
}
