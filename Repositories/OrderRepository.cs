using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.Data;
using RestaurantManagementSystem.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Order>> GetOrdersWithDetailsAsync()
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.MenuItem)
                .Include(o => o.OrderPromotions)
                    .ThenInclude(op => op.Promotion)
                .Include(o => o.OrderTables)
                    .ThenInclude(ot => ot.Table)
                .ToListAsync();
        }

        public async Task<Order> GetOrderByIdWithDetailsAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.MenuItem)
                .Include(o => o.OrderPromotions)
                    .ThenInclude(op => op.Promotion)
                .Include(o => o.OrderTables)
                    .ThenInclude(ot => ot.Table)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }
    }
}