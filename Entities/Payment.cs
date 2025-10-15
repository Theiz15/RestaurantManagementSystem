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

        [Required]
        [MaxLength(100)]
        [Column("payment_method")]
        public PaymentMethod PaymentMethod { get; set; }

        [Column("amount_paid", TypeName = "decimal(10, 2)")]
        public decimal AmountPaid { get; set; }

        [Column("status")]
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

        [MaxLength(100)]
        public string? VnpTransactionNo { get; set; }

        [MaxLength(10)]
        public string? VnpResponseCode { get; set; }

        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }

        [ForeignKey("OrderId")]
        public Order? Order { get; set; }
    }
}