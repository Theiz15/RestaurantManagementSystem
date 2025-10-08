using RestaurantManagementSystem.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManagementSystem.Models
{
    public class InventoryItem
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        [Column("name")]
        public string Name { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("unit")]
        public string Unit { get; set; }

        public int InventoryId { get; set; }


        [ForeignKey("InventoryId")]
        public virtual Inventory Inventory { get; set; }
    }
}
