namespace RestaurantManagementSystem.DTOs.Responses
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? RoleName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
