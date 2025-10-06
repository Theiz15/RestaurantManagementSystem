using RestaurantManagementSystem.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManagementSystem.Models
{
    [Table("tables")]
    public class Table
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("table_number")]
        public string? TableNumber { get; set; }

        [Column("capacity")]
        public int Capacity { get; set; }

        [MaxLength(255)]
        [Column("status")]
        public TableStatus Status { get; set; }

        [MaxLength(255)]
        [Column("location")]
        public string? Location { get; set; }

        public ICollection<OrderTable>? OrderTables { get; set; }
        public ICollection<Reservation>? Reservations { get; set; }
    }
}