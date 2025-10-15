using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.DTOs.Responses
{
    public class PaymentResponseModel
    {
        public string? OrderDescription { get; set; }
        // Add transactionNo
        public string? TransactionNo { get; set; }
        public string? PaymentMethod { get; set; }
        public int PaymentId { get; set; }
        public bool Success { get; set; }
        public string? Token { get; set; }
        public string? VnPayResponseCode { get; set; }
    }
 

}