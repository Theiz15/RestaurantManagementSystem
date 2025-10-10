using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.DTOs.Requests
{
    public class ShiftAssignmentRequest
    {
        public int UserId { get; set; }
        public int ShiftId { get; set; }
        public DateTime WorkDate { get; set; }
    }
}