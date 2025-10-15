using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.DTOs.Responses
{
    public class VnPayResponse
    {
        public bool Success { get; set; }
        public string? PaymentUrl { get; set; }
        public string? Message { get; set; }
    }
}