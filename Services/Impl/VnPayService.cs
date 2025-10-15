using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestaurantManagementSystem.DTOs.Requests;
using RestaurantManagementSystem.DTOs.Responses;
using RestaurantManagementSystem.Libraries;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Repositories;

namespace RestaurantManagementSystem.Services.Impl
{
    public class VnPayService : IVnPayService
    {
        private readonly IConfiguration _configuration;
        private readonly PaymentRepository _paymentRepository;
        public VnPayService(IConfiguration configuration, PaymentRepository paymentRepository)
        {
            _configuration = configuration;
            _paymentRepository = paymentRepository;
        }
        public async Task<string> CreatePaymentUrl(PaymentInformationModel model, HttpContext context)
        {
            //Check order exist???
            //Get user, user Id from order

            //Create payment
            var payment = new Payment
            {
                OrderId = model.OrderId,
                PaymentMethod = Enums.PaymentMethod.VNPay,
                AmountPaid = (decimal)model.Amount,
                CreateAt = DateTime.Now,
                Status = Enums.PaymentStatus.Pending
            };

            await _paymentRepository.AddAsync(payment);
            await _paymentRepository.SaveChangesAsync();

            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["Payment:TimeZoneId"]);
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
            // var tick = DateTime.Now.Ticks.ToString();
            var pay = new VnPayLibrary();
            var urlCallBack = _configuration["Payment:Vnpay:PaymentCallBack"];

            pay.AddRequestData("vnp_Version", _configuration["Payment:Vnpay:Version"]);
            pay.AddRequestData("vnp_Command", _configuration["Payment:Vnpay:Command"]);
            pay.AddRequestData("vnp_TmnCode", _configuration["Payment:Vnpay:TmnCode"]);
            pay.AddRequestData("vnp_Amount", ((int)model.Amount * 100).ToString());
            pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", _configuration["Payment:Vnpay:CurrCode"]);
            pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
            pay.AddRequestData("vnp_Locale", _configuration["Payment:Vnpay:Locale"]);
            pay.AddRequestData("vnp_OrderInfo", $"{model.Name} {model.OrderDescription} {model.Amount}");
            pay.AddRequestData("vnp_OrderType", model.OrderType);
            pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
            pay.AddRequestData("vnp_TxnRef", payment.Id.ToString()); // Change tick by payment id;

            var paymentUrl =
                pay.CreateRequestUrl(_configuration["Payment:Vnpay:BaseUrl"], _configuration["Payment:Vnpay:HashSecret"]);

            return paymentUrl;
        }



        public async Task<PaymentResponseModel> PaymentExecute(IQueryCollection collections)
        {
            var pay = new VnPayLibrary();
            var response = pay.GetFullResponseData(collections, _configuration["Payment:Vnpay:HashSecret"]);

            var payment = _paymentRepository.FindById(response.PaymentId);
            if (payment == null) throw new System.Exception("Not find payment!");

            if (response.Success)
            {
                payment.Status = Enums.PaymentStatus.Paid;
                // Set order status = paid
                // Set table status = Available
            }
            else
            {
                payment.Status = Enums.PaymentStatus.Fail;
            }
            
            payment.VnpTransactionNo = response.TransactionNo;
            payment.VnpResponseCode = response.VnPayResponseCode;
            payment.UpdateAt = DateTime.Now;

            await _paymentRepository.SaveChangesAsync();

            return response;
        }
    }
}