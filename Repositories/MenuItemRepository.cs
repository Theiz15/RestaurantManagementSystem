using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.Data;
using RestaurantManagementSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Repositories
{
    public class MenuItemRepository : GenericRepository<MenuItem>, IMenuItemRepository
    {
        public MenuItemRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<MenuItem>> GetAllWithCategoryAsync()
        {
            return await _context.MenuItems.Include(m => m.Category).ToListAsync();
        }

        public async Task<MenuItem?> GetByIdWithCategoryAsync(int id)
        {
            return await _context.MenuItems.Include(m => m.Category)
            .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<MenuItem?> GetByNameAsync(string name)
        {
            return await _context.MenuItems
            .FirstOrDefaultAsync(m => m.ProductName.ToLower() == name.ToLower());
        }
    }
}