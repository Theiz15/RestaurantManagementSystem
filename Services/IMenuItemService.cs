using RestaurantManagementSystem.DTOs.Requests;


namespace RestaurantManagementSystem.Services
{
    public interface IMenuItemService
    {
        Task<IEnumerable<MenuItemDTO>> GetAllMenuItemsAsync();
        Task<MenuItemDTO> GetMenuItemByIdAsync(int id);
        Task<MenuItemDTO> CreateMenuItemAsync(MenuItemDTO menuItemDto);
        Task<bool> UpdateMenuItemAsync(int id, MenuItemDTO menuItemDto);
        Task<bool> DeleteMenuItemAsync(int id);
    }
}