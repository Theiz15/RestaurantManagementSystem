using RestaurantManagementSystem.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManagementSystem.Models
{
    [Table("shifts")]
    public class Shift
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("start_time")]
        public DateTime StartTime { get; set; }

        [Required]
        [Column("end_time")]
        public DateTime EndTime { get; set; }

        [Required]
        [Column("status")]
        public ShiftStatus Status { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [Required]
        [Column("shift_date")]
        public DateTime ShiftDate { get; set; }

        public ICollection<ShiftAssignment> ShiftAssignments { get; set; }
    }
}