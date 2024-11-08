using BOs.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAOs
{
    public class AccountDAO : IAccountDAO
    {
        private readonly BookingRoommContext _context;

        public AccountDAO(BookingRoommContext context)
        {
            _context = context;
        }

        public async Task<Guest?> GetGuestByEmailAsync(string email)
        {
            return await _context.Guests.FirstOrDefaultAsync(g => g.Email == email);
        }

        public async Task UpdateGuestAsync(Guest guest)
        {
            _context.Guests.Update(guest);
            await _context.SaveChangesAsync();
        }
    }
}
