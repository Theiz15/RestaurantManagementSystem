using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestaurantManagementSystem.Enums;

namespace RestaurantManagementSystem.DTOs.Requests
{
    public class FilterShiftAssignmentRequest
    {
        public int userId { get; set; }
        public ShiftStatus status { get; set; }
    }
}