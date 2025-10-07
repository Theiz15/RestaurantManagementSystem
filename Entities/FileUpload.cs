using RestaurantManagementSystem.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManagementSystem.Models
{
    [Table("file_uploads")]
    public class FileUpload
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("menu_item_id")]
        public int? MenuItemId { get; set; } // Link to menu item if it's an image of a dish

        [Column("name")]
        public string? Name { get; set; }

        [Required]
        [Column("path")]
        public string? Path { get; set; }

        [Column("location")]
        public string? Location { get; set; }

        [Column("file_type")]
        public FileType FileType { get; set; }

        [ForeignKey("MenuItemId")]
        public MenuItem? MenuItem { get; set; }

        public Category? Category { get; set; }
    }




}