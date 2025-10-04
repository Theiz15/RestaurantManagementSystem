using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManagementSystem.Models
{
    [Table("Promotions")]
    public class Promotion
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("code")]
        public string? Code { get; set; }

        [Required]
        [Column("name", TypeName = "text")]
        public string? Name { get; set; }

        [Column("description", TypeName = "text")]
        public string? Description { get; set; }

        [Column("discount", TypeName = "decimal(5, 2)")]
        public decimal Discount { get; set; }

        [Column("start_date")]
        public DateTime StartDate { get; set; }

        [Column("end_date")]
        public DateTime EndDate { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }

        public ICollection<OrderPromotion>? OrderPromotions { get; set; }
    }
}