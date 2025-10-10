using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.Data;
using RestaurantManagementSystem.Models;

namespace RestaurantManagementSystem.Repositories
{
    public class ShiftRepository : GenericRepository<Shift>
    {
        private readonly AppDbContext _context;
        public ShiftRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Shift> findByNameAsync(string name)
        {
            return await _context.Shifts.FirstOrDefaultAsync(s => s.Name == name);
        }

        public async Task<Shift?> FindByIdAsync(int id)
        {
            return await _context.Shifts.FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}