using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.Data;
using RestaurantManagementSystem.Models;
using System.Linq.Expressions;

namespace RestaurantManagementSystem.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;
        public CategoryRepository(AppDbContext context) { _context = context; }

        public async Task<IEnumerable<Category>> GetAllAsync() => await _context.Categories.AsNoTracking().ToListAsync();
        public async Task<Category> GetByIdAsync(int id) => await _context.Categories.FindAsync(id);
        public async Task AddAsync(Category category) => await _context.Categories.AddAsync(category);
        public void Update(Category category) => _context.Categories.Update(category);
        public void Delete(Category category) => _context.Categories.Remove(category);
        public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;

        public async Task<Category> GetByNameAsync(string? name)
        {
            return await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Name == name);
        }

        Task IGenericRepository<Category>.SaveChangesAsync()
        {
            return SaveChangesAsync();
        }

        public Task<Category?> SingleOrDefaultAsync(Expression<Func<Category, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task AddRangeAsync(IEnumerable<Category> entities)
        {
            throw new NotImplementedException();
        }
    }
}
