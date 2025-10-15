using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestaurantManagementSystem.DTOs.Requests;
using RestaurantManagementSystem.DTOs.Responses;

namespace RestaurantManagementSystem.Services
{
    public interface IVnPayService
    {
        Task<string> CreatePaymentUrl(PaymentInformationModel model, HttpContext context);

        Task<PaymentResponseModel> PaymentExecute(IQueryCollection collections);


    }
}