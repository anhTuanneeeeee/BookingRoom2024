using BOs.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAOs
{
    public class StatusDAO : IStatusDAO
    {
        private readonly BookingRoommContext _context;

        public StatusDAO(BookingRoommContext context)
        {
            _context = context;
        }

        public async Task<Status> CreateStatusAsync(Status status)
        {
            _context.Statuses.Add(status);
            await _context.SaveChangesAsync();
            return status;
        }

        public async Task<IEnumerable<Status>> GetAllStatusAsync()
        {
            return await _context.Statuses.ToListAsync();
        }

        public async Task<Status?> GetStatusByIdAsync(int statusId)
        {
            return await _context.Statuses.FirstOrDefaultAsync(s => s.StatusId == statusId);
        }
        
    }
}
