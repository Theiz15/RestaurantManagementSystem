using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManagementSystem.Models
{
    [Table("order_promotions")]
    public class OrderPromotion
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("order_id")]
        public int OrderId { get; set; }

        [Column("promotion_id")]
        public int PromotionId { get; set; }

        [Column("applied_at")]
        public DateTime AppliedAt { get; set; }

        [ForeignKey("OrderId")]
        public Order Order { get; set; }

        [ForeignKey("PromotionId")]
        public Promotion Promotion { get; set; }
    }
}