using RestaurantManagementSystem.Enums;

namespace RestaurantManagementSystem.DTOs.Responses
{
    public class OrderResponse
    {
        public int Id { get; set; }
        public string? CustomerName { get; set; }
        public OrderStatus Status { get; set; }
        public decimal TotalAmount { get; set; }
        public string? OrderType { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<OrderItemResponse>? Items { get; set; }
        public List<string>? TableNumbers { get; set; }
    }
}
