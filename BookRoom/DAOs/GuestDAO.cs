using BOs.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAOs
{
     public class GuestDAO : IGuestDAO
    {
        private readonly BookingRoommContext _context;

        public GuestDAO(BookingRoommContext context)
        {
            _context = context;
        }

        public async Task<Guest> CreateGuest(Guest guest)
        {
            _context.Guests.Add(guest);
            await _context.SaveChangesAsync();
            return guest;
        }

        public async Task<Guest?> GetGuestByEmail(string email)
        {
            return await _context.Guests.FirstOrDefaultAsync(g => g.Email == email);
        }
        public async Task<Guest?> GetGuestByPhoneNumber(string phoneNumber)
        {
            return await _context.Guests.FirstOrDefaultAsync(g => g.PhoneNumber == phoneNumber);
        }
        public async Task<IEnumerable<Guest>> GetAllGuestsAsync()
        {
            return await _context.Guests.ToListAsync();
        }

        public async Task<Guest> GetGuestByIdAsync(int id)
        {
            return await _context.Guests.FindAsync(id);
        }

        public async Task<Guest> CreateGuestAsync(Guest guest)
        {
            _context.Guests.Add(guest);
            await _context.SaveChangesAsync();
            return guest;
        }
        public async Task<Guest?> GetGuestById(int id)
        {
            return await _context.Guests.FirstOrDefaultAsync(g => g.Id == id);
        }
        public async Task<Guest> UpdateGuest(Guest guest)
        {
            _context.Guests.Update(guest);
            await _context.SaveChangesAsync();
            return guest;
        }
        public async Task<bool> DeleteGuest(int id)
        {
            var guest = await _context.Guests.FindAsync(id);
            if (guest == null)
            {
                return false;
            }

            _context.Guests.Remove(guest);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
