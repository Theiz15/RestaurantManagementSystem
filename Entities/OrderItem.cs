using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManagementSystem.Models
{
    [Table("order_items")]
    public class OrderItem
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("order_id")]
        public int OrderId { get; set; }

        [Column("menu_item_id")]
        public int MenuItemId { get; set; }

        [Required]
        [Column("quantity")]
        public int Quantity { get; set; }

        [Column("subtotal", TypeName = "decimal(18, 2)")]
        public decimal Subtotal { get; set; }

        [Column("added_at")]
        public DateTime AddedAt { get; set; }

        public Order Order { get; set; }
        public MenuItem MenuItem { get; set; }
    }
}