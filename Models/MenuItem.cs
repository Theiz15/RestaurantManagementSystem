using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace RestaurantManagementSystem.Models
{
    [Table("menu_items")]
    public class MenuItem
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        [Column("product_name")]
        public string ProductName { get; set; }

        [Column("description", TypeName = "text")]
        public string Description { get; set; }

        [Column("price", TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("is_available")]
        public bool IsAvailable { get; set; }

        [Column("category_id")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
        public ICollection<Inventory> Inventories { get; set; } // Assuming Menu Items can be linked to inventory for stock tracking
        public ICollection<FileUpload> FileUploads { get; set; }
    }
}