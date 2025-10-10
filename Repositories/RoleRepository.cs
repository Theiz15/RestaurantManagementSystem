using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.Data;
using RestaurantManagementSystem.Models;

namespace RestaurantManagementSystem.Repositories
{
    public class RoleRepository : GenericRepository<Role>
    {
        private  readonly AppDbContext _context;
        public RoleRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Role> GetRoleByRoleId(int roleId)
        {
            return await _context.Roles.FindAsync(roleId);
        }

        public async Task<Role> GetRoleByName(string roleName)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
        }

        internal object GetRoleByRoleId(int? roleId)
        {
            throw new NotImplementedException();
        }
    }
}