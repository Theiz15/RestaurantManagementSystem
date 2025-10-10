using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.Data;
using RestaurantManagementSystem.Enums;
using RestaurantManagementSystem.Models;

namespace RestaurantManagementSystem.Repositories
{
    public class ShiftAssignmentRepository : GenericRepository<ShiftAssignment>
    {
        private readonly AppDbContext _context;
        public ShiftAssignmentRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ShiftAssignment?> FindByUserIdAndShiftId(int userId, int ShiftId, DateTime dateTime)
        {
            return await _context.ShiftAssignments
                  .FirstOrDefaultAsync(sa => sa.UserId == userId && sa.ShiftId == ShiftId && sa.WorkDate == dateTime);
        }

        public async Task<ShiftAssignment?> FindByIdAsync(int id)
        {
            return await _context.ShiftAssignments
                   .Include(sa => sa.User)
                   .Include(sa => sa.Shift)
                   .FirstOrDefaultAsync(sa => sa.Id == id);
        }

        public async Task<ShiftAssignment?> FindByUserIdAndDate(int userId, DateTime date)
        {
            return await _context.ShiftAssignments
                    .FirstOrDefaultAsync(sa => sa.UserId == userId && sa.WorkDate == date);
        }
        
        public async Task<ShiftAssignment?> FindByUserIdAndAssignmentId (int userId, int assignmentId)
        {
            return await _context.ShiftAssignments.FirstOrDefaultAsync(sa => sa.UserId == userId && sa.Id == assignmentId);
        }

        internal void Delete(Task<ShiftAssignment?> shiftAssignment)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ShiftAssignment>> GetAllByUserIdAsync(int userId)
        {
            return await _context.ShiftAssignments
                .Include(sa => sa.Shift)
                .Where(sa => sa.UserId == userId)
                .OrderByDescending(sa => sa.WorkDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<ShiftAssignment>> FilterShiftAssignment (int userId, ShiftStatus status)
        {
            return await _context.ShiftAssignments
                .Include(sa => sa.Shift)
                .Where(sa => sa.UserId == userId && sa.Status == status)
                .ToListAsync();
        }
    }
}