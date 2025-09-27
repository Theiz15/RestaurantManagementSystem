using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManagementSystem.Models
{
    [Table("inventories")]
    public class Inventory
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        //public int MenuItemId { get; set; } // Assuming inventory is for menu items

        [Required]
        [Column("item_name",TypeName="text")]
        public string ItemName { get; set; } // Denormalized for convenience

        [Required]
        [Column("quantity", TypeName = "decimal(18, 3)")]
        public int Quantity { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("unit")]
        public string Unit { get; set; }

        [Column("min_threshold", TypeName = "decimal(18, 3)")]
        public int MinThreshold { get; set; }

        [Column("last_updated")]
        public DateTime LastUpdated { get; set; }



        //public MenuItem MenuItem { get; set; }
        public ICollection<InventoryTransaction> InventoryTransactions { get; set; }
    }
}