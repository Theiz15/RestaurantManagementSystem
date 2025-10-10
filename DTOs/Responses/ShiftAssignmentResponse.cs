using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestaurantManagementSystem.Enums;

namespace RestaurantManagementSystem.DTOs.Responses
{
    public class ShiftAssignmentResponse
    {
        public int Id { get; set; }
        public int ShiftId { get; set; }
        public int UserId { get; set; }
        public DateTime? WorkDate { get; set; }
        public DateTime? ActualStartTime { get; set; }
        public DateTime? ActualEndTime { get; set; }
        public decimal HoursWorked { get; set; }
        public ShiftStatus Status { get; set; }
    }
}