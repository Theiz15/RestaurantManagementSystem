using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.Data;
using RestaurantManagementSystem.Models;

namespace RestaurantManagementSystem.Repositories
{
    public class PaymentRepository : GenericRepository<Payment>
    {
        private readonly AppDbContext _context;

        public PaymentRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Payment> FindByOrderId(int orderId)
        {
            return await _context.Payments
                    .FirstOrDefaultAsync(p => p.OrderId == orderId);
        }
        
        public Payment FindById (int id)
        {
            return _context.Payments.FirstOrDefault(p => p.Id == id);
        }

        internal async Task UpdateAsync(Payment payment)
        {
            throw new NotImplementedException();
        }
    }
}