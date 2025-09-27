namespace RestaurantManagementSystem.DTOs.Responses
{
    public class OrderItemResponse
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal { get; set; }
    }
}
