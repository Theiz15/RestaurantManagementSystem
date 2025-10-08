using RestaurantManagementSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Repositories
{
    public interface IInventoryRepository
    {
        Task<InventoryItem> GetInventoryItemByNameAsync(int inventoryId, string name);
        Task<bool> InventoryExistsByNameAsync(string name);
        Task<IEnumerable<Inventory>> GetAllInventoriesAsync();
        Task<Inventory> GetInventoryByIdAsync(int inventoryId);
        Task AddInventoryAsync(Inventory inventory);
        Task UpdateInventoryAsync(Inventory inventory);
        Task DeleteInventoryAsync(int inventoryId);

        Task<IEnumerable<InventoryItem>> GetInventoryItemsByInventoryIdAsync(int inventoryId);
        Task<InventoryItem> GetInventoryItemByIdAsync(int inventoryItemId);
        Task AddInventoryItemAsync(InventoryItem inventoryItem);
        Task UpdateInventoryItemAsync(InventoryItem inventoryItem);
        Task DeleteInventoryItemAsync(int inventoryItemId);
    }
}