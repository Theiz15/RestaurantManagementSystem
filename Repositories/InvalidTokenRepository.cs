using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.Data;
using RestaurantManagementSystem.Entities;

namespace RestaurantManagementSystem.Repositories
{
    public class InvalidTokenRepository : GenericRepository<InvalidToken>
    {
        private readonly AppDbContext _context;
        public InvalidTokenRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> ExistsAsync(string jti)
        {
            return await _context.InvalidTokens.AnyAsync(t => t.Id == jti);
        }
    }
}