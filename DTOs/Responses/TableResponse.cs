using RestaurantManagementSystem.Enums;

namespace RestaurantManagementSystem.DTOs.Responses
{
    public class TableResponse
    {
        public int Id { get; set; }

        //public TableType tableType;

        public int Capacity { get; set; }

        public TableStatus Status { get; set; }

    }
}
