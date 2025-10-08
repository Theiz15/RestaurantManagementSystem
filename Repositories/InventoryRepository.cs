using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.Data;
using RestaurantManagementSystem.Migrations;
using RestaurantManagementSystem.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly AppDbContext _context; // Đổi tên DbContext

        public InventoryRepository(AppDbContext context) // Đổi tên DbContext
        {
            _context = context;
        }

        // ... (các phương thức Inventory và InventoryItem như đã cung cấp trước đó)
        // Đảm bảo GetInventoryByIdAsync và GetAllInventoriesAsync include InventoryItems
        public async Task<IEnumerable<Inventory>> GetAllInventoriesAsync()
        {
            return await _context.Inventories.Include(i => i.InventoryItems).ToListAsync();
        }
        public async Task<InventoryItem> GetInventoryItemByNameAsync(int inventoryId, string name)
        {
            // Tìm item đầu tiên khớp với cả inventoryId và tên (không phân biệt hoa thường)
            return await _context.InventoryItems
                .FirstOrDefaultAsync(ii => ii.InventoryId == inventoryId && ii.Name.ToLower() == name.ToLower());
        }

        public async Task<Inventory> GetInventoryByIdAsync(int inventoryId)
        {
            return await _context.Inventories
                                 .Include(i => i.InventoryItems)
                                 .FirstOrDefaultAsync(i => i.Id == inventoryId);
        }

        public async Task<bool> InventoryExistsByNameAsync(string name)
        {
            // Sử dụng ToLower() để kiểm tra không phân biệt chữ hoa, chữ thường
            // Điều này giúp tránh việc tạo ra "Thịt Bò" khi đã có "thịt bò"
            return await _context.Inventories.AnyAsync(i => i.ItemName.ToLower() == name.ToLower());
        }

        public async Task AddInventoryAsync(Inventory inventory)    
        {
            await _context.Inventories.AddAsync(inventory);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateInventoryAsync(Inventory inventory)
        {
            _context.Inventories.Update(inventory);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteInventoryAsync(int inventoryId)
        {
            var inventory = await _context.Inventories.FindAsync(inventoryId);
            if (inventory != null)
            {
                _context.Inventories.Remove(inventory);
                await _context.SaveChangesAsync();
            }
        }

        // InventoryItem
        public async Task<IEnumerable<InventoryItem>> GetInventoryItemsByInventoryIdAsync(int inventoryId)
        {
            return await _context.InventoryItems
                                 .Where(ii => ii.InventoryId == inventoryId)
                                 .ToListAsync();
        }

        public async Task<InventoryItem> GetInventoryItemByIdAsync(int inventoryItemId)
        {
            return await _context.InventoryItems
                                 .FirstOrDefaultAsync(ii => ii.Id == inventoryItemId);
        }

        public async Task AddInventoryItemAsync(InventoryItem inventoryItem)
        {
            await _context.InventoryItems.AddAsync(inventoryItem);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateInventoryItemAsync(InventoryItem inventoryItem)
        {
            _context.InventoryItems.Update(inventoryItem);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteInventoryItemAsync(int inventoryItemId)
        {
            var inventoryItem = await _context.InventoryItems.FindAsync(inventoryItemId);
            if (inventoryItem != null)
            {
                _context.InventoryItems.Remove(inventoryItem);
                await _context.SaveChangesAsync();
            }
        }
    }
}