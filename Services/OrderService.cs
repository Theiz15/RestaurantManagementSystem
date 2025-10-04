using AutoMapper;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Repositories;
using RestaurantManagementSystem.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestaurantManagementSystem.DTOs.Requests;

namespace RestaurantManagementSystem.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMenuItemRepository _menuItemRepository; // Assuming you create this
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, IMenuItemRepository menuItemRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _menuItemRepository = menuItemRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderDTO>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetOrdersWithDetailsAsync();
            return _mapper.Map<IEnumerable<OrderDTO>>(orders);
        }

        public async Task<OrderDTO> GetOrderByIdAsync(int id)
        {
            var order = await _orderRepository.GetOrderByIdWithDetailsAsync(id);
            return _mapper.Map<OrderDTO>(order);
        }

        public async Task<OrderDTO> CreateOrderAsync(OrderDTO orderDto)
        {
            var order = _mapper.Map<Order>(orderDto);
            order.CreatedAt = DateTime.UtcNow;
            order.Status = OrderStatus.Pending; // Default status

            decimal totalAmount = 0;
            foreach (var itemDto in orderDto.Items)
            {
                var menuItem = await _menuItemRepository.GetByIdAsync(itemDto.MenuItemId);
                if (menuItem != null && menuItem.IsAvailable)
                {
                    var orderItem = new OrderItem
                    {
                        MenuItemId = itemDto.MenuItemId,
                        Quantity = itemDto.Quantity,
                        Subtotal = menuItem.Price * itemDto.Quantity
                    };
                    totalAmount += orderItem.Subtotal;
                    order.OrderItems.Add(orderItem);
                }
                else
                {
                    // Handle case where menu item is not found or not available
                    throw new SystemException($"Menu item with ID {itemDto.MenuItemId} is not available.");
                }
            }
            order.TotalAmount = totalAmount;

            // Handle tables (assuming OrderTable has been configured in DbContext)
            if (orderDto.TableIds != null)
            {
                order.OrderTables = orderDto.TableIds.Select(tableId => new OrderTable { TableId = tableId }).ToList();
            }

            // Handle promotions (logic to be implemented)

            await _orderRepository.AddAsync(order);
            await _orderRepository.SaveChangesAsync();

            var createdOrder = await _orderRepository.GetOrderByIdWithDetailsAsync(order.Id);
            return _mapper.Map<OrderDTO>(createdOrder);
        }

        //public async Task<bool> UpdateOrderStatusAsync(int id, OrderUpdateStatusDTO statusDto)
        //{
        //    var order = await _orderRepository.GetByIdAsync(id);
        //    if (order == null)
        //    {
        //        return false;
        //    }

        //    order.Status = statusDto.Status;
        //    order.UpdatedAt = DateTime.UtcNow;

        //    _orderRepository.Update(order);
        //    await _orderRepository.SaveChangesAsync();
        //    return true;
        //}

        public async Task<bool> DeleteOrderAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
            {
                return false;
            }

            _orderRepository.Delete(order);
            await _orderRepository.SaveChangesAsync();
            return true;
        }
    }
}