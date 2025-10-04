using RestaurantManagementSystem.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManagementSystem.Models
{
    [Table("reservations")]
    public class Reservation
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("customer_id")]
        public int CustomerId { get; set; }

        [Column("table_id")]
        public int TableId { get; set; }

        [Column("number_of_people")]
        public int NumberOfPeople { get; set; }

        [Column("arive_at")]
        public DateTime ArriveAt { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("status")]
        public ReservationStatus Status { get; set; } // Enum ReservationStatus
        
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }


        [ForeignKey("CustomerId")]
        public User? Customer { get; set; }

        [ForeignKey("TableId")]
        public Table? Table { get; set; }
    }
}