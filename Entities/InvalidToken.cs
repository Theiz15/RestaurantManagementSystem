using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Entities
{
    public class InvalidToken
    {
        [Key]
        public string? Id { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}