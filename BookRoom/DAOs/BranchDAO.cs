using BOs.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAOs
{
    public class BranchDAO : IBranchDAO
    {
        private readonly BookingRoommContext _context;

        public BranchDAO(BookingRoommContext context)
        {
            _context = context;
        }

        public async Task AddBranch(Branch branch)
        {
            await _context.Branches.AddAsync(branch);
            await _context.SaveChangesAsync();
        }

        public async Task<Branch?> GetBranchById(int id)
        {
            return await _context.Branches
        .Include(b => b.Rooms) // Tải Rooms liên kết
        .FirstOrDefaultAsync(b => b.BranchId == id);
        }

        public async Task<List<Branch>> GetAllBranches()
        {
            return await _context.Branches
         .Include(b => b.Rooms) // Nạp phòng cùng với chi nhánh
         .ToListAsync();
        }

        public async Task<List<Branch>> SearchBranches(string searchTerm)
        {
            return await _context.Branches
                .Where(b => b.BranchName.Contains(searchTerm) || b.Location.Contains(searchTerm))
                .Include(b => b.Rooms)
                .ToListAsync();
        }

        public async Task DeleteBranch(int id)
        {
            var branch = await _context.Branches.FindAsync(id);
            if (branch != null)
            {
                _context.Branches.Remove(branch);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateBranch(Branch branch)
        {
            _context.Branches.Update(branch);
            await _context.SaveChangesAsync();
        }
    }

}
