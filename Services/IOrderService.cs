using RestaurantManagementSystem.DTOs.Requests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDTO>> GetAllOrdersAsync();
        Task<OrderDTO> GetOrderByIdAsync(int id);
        Task<OrderDTO> CreateOrderAsync(OrderDTO orderDto);
        Task<bool> DeleteOrderAsync(int id);
    }
}