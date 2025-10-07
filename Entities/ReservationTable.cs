using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManagementSystem.Models
{
    [Table("Reservation_Tables")]
    public class ReservationTable
    {
        [Column("table_id")]
        public int TableId { get; set; }

        [Column("reservation_id")]
        public int ReservationId { get; set; }

        [ForeignKey("TableId")]
        public Table? Table { get; set; }

        [ForeignKey("ReservationId")]
        public Reservation? Reservation { get; set; }
    }
}
