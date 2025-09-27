using RestaurantManagementSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Repositories
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<IEnumerable<Order>> GetOrdersWithDetailsAsync();
        Task<Order> GetOrderByIdWithDetailsAsync(int orderId);
        // Add more order-specific methods
    }
}