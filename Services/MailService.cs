using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Services
{
    public interface MailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }
}