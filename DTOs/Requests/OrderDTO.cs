namespace RestaurantManagementSystem.DTOs.Requests
{
    public class OrderDTO
    {
        public int? UserId { get; set; } // Dành cho khách hàng đã đăng nhập
        public string? OrderType { get; set; }
        public DateTime ExpectedTime { get; set; }
        public List<OrderItemDTO>? Items { get; set; }
        public List<int>? TableIds { get; set; }
    }
}
