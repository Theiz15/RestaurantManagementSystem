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

        [Column("name")]
        public string? Name { get; set; }

        [Required]
        [Column("start_time")]
        public TimeSpan StartTime { get; set; }

        [Required]
        [Column("end_time")]
        public TimeSpan EndTime { get; set; }

        [Required]
        [Column("status")]
        public ShiftStatus Status { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        public ICollection<ShiftAssignment> ShiftAssignments { get; set; } = new List<ShiftAssignment>();
    }
}