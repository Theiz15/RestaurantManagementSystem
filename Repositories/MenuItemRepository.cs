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

        public async Task<IEnumerable<MenuItem>> GetMenuItemsWithCategoryAsync()
        {
            return await _context.MenuItems.Include(m => m.Category).ToListAsync();
        }
    }
}