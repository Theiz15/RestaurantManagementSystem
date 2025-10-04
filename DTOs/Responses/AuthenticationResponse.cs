using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.DTOs.Responses
{
    public class AuthenticationResponse
    {
        public bool successful { get; set; }
        public string? token { get; set; }
    }
}