using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RestaurantManagementSystem.Enums;

namespace RestaurantManagementSystem.Models
{
    [Table("shift_assignment")]
    public class ShiftAssignment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("shift_id")]
        public int ShiftId { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("work_date")]
        public DateTime? WorkDate { get; set; }

        [Column("actual_start_time")]
        public DateTime? ActualStartTime { get; set; }

        [Column("actual_end_time")]
        public DateTime? ActualEndTime { get; set; }

        [Column("hours_worked", TypeName = "decimal(18, 3)")]
        public decimal HoursWorked { get; set; }
        
        [Column("status")]
        public ShiftStatus? Status { get; set; }

        [ForeignKey("ShiftId")]
        public Shift? Shift { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }
    }
}