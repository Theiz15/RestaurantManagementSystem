using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using RestaurantManagementSystem.Enums;

namespace RestaurantManagementSystem.Models
{
    [Table("users")]
    public class User
    {
        [Key] 
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("username")]
        public string Username { get; set; }

        [MaxLength(255)]
        [Column("password")]
        public string Password { get; set; }

        [MaxLength(255)]
        [Column("full_name")]
        public string FullName { get; set; }

        [MaxLength(255)]
        [Column("email")]
        public string Email { get; set; }

        [MaxLength(20)]
        [Column("phone")]
        public string Phone { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [Column("role_id")]
        public int? RoleId { get; set; }

        [ForeignKey("RoleId")]
        public Role? Role { get; set; }


        [Column("status")]
        public UserStatus? Status { get; set; }

        public ICollection<ShiftAssignment>? ShiftAssignments { get; set; }
        public ICollection<Order>? Orders { get; set; }
        public ICollection<Reservation>? Reservations { get; set; }
        public ICollection<Payment>? Payments { get; set; } // Cashier
    }
}