using RestaurantManagementSystem.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManagementSystem.Models
{
    [Table("Payments")]
    public class Payment
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("order_id")]
        public int OrderId { get; set; }

        [Column("cashier_id")]
        public int? CashierId { get; set; } // User who processed the payment

        [Required]
        [MaxLength(100)]
        [Column("payment_method")]
        public PaymentMethod PaymentMethod { get; set; }

        [Column("amount_paid", TypeName = "decimal(10, 2)")]
        public decimal AmountPaid { get; set; }

        [Column("timestamp")]
        public DateTime Timestamp { get; set; }

        [ForeignKey("OrderId")]
        public Order? Order { get; set; }

        [ForeignKey("CashierId")]
        public User? Cashier { get; set; }
    }
}