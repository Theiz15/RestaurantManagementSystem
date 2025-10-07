namespace RestaurantManagementSystem.DTOs.Responses
{
    public class MenuItemResponse
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        public string CategoryName { get; set; } // Ta sẽ lấy tên Category để hiển thị
        public DateTime CreatedAt { get; set; }
    }
}
