using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManagementSystem.DTOs.Responses
{
    public class InventoryItemResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; }

        public string Unit { get; set; }
    }
}
