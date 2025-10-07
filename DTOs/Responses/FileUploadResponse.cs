namespace RestaurantManagementSystem.DTOs.Responses
{
    public class FileUploadResponse
    {
        public int Id { get; set; }
        public string OriginalName { get; set; }
        public string Url { get; set; } 
        public string FileType { get; set; } 
        public int? MenuItemId { get; set; }
    }
}
