using RestaurantManagementSystem.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManagementSystem.Models
{
    [Table("orders")]
    public class Order
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("user_id")]
        public int UserId { get; set; } // User who placed the order or staff who created it

        [Column("order_type")]
        public OrderType OrderType { get; set; } // Enum OrderType

        [Column("status")]
        public OrderStatus Status { get; set; } // Enum OrderStatus

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("expected_time")]
        public DateTime? ExpectedTime { get; set; }

        [Required]
        [Column("total_amount", TypeName = "decimal(18, 2)")]
        public decimal TotalAmount { get; set; }

        [Column("deposit_amount", TypeName = "decimal(18, 2)")]
        public decimal DepositAmount { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
        public ICollection<OrderTable> OrderTables { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public ICollection<Payment> Payments { get; set; }
        public ICollection<OrderPromotion> OrderPromotions { get; set; }
    }
}