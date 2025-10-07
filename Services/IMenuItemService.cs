using RestaurantManagementSystem.DTOs.Requests;
using RestaurantManagementSystem.DTOs.Responses;


namespace RestaurantManagementSystem.Services
{
    public interface IMenuItemService
    {
        Task<IEnumerable<MenuItemResponse>> GetAllMenuItemsAsync();
        Task<MenuItemResponse> GetMenuItemByIdAsync(int id);
        Task<MenuItemResponse> CreateMenuItemAsync(MenuItemCreateDTO menuItemDto);
        Task<MenuItemResponse> UpdateMenuItemAsync(int id, MenuItemUpdateDTO menuItemDto);
        Task DeleteMenuItemAsync(int id);
    }
}