// Dtos/InventoryItemCreateDto.cs
using System.ComponentModel.DataAnnotations;

namespace RestaurantManagementSystem.DTOs
{
    public class InventoryItemCreateDTO
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        public int Quantity { get; set; }

        [Required]
        [MaxLength(50)]
        public string Unit { get; set; }
    }
}