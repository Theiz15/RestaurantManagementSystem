using RestaurantManagementSystem.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManagementSystem.Models
{
    [Table("inventory_transaction")]
    public class InventoryTransaction
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("inventory_id")]
        public int InventoryId { get; set; }

        [Required]
        [Column("quantity")]
        public int Quantity { get; set; }

        [Required]
        [Column("transaction_type")]
        public InventoryTransactionType TransactionType { get; set; } // Enum InventoryTransactionType

        [Column("timestamp")]
        public DateTime Timestamp { get; set; }

        [ForeignKey("InventoryId")]
        public Inventory Inventory { get; set; }
    }
}