using BOs.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAOs
{
    public class BookingDAO : IBookingDAO
    {
        private readonly BookingRoommContext _context;

        public BookingDAO(BookingRoommContext context)
        {
            _context = context;
        }

        public async Task<Booking> CreateBookingAsync(Booking booking)
        {
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            return booking;
        }
        public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
        {
            return await _context.Bookings
                .Include(b => b.Room) 
                .Include(b => b.SlotBookings) 
                .ToListAsync();
        }

        public async Task<Booking?> GetBookingByIdAsync(int bookingId)
        {
            return await _context.Bookings
                .Include(b => b.Room) 
                .Include(b => b.SlotBookings) 
                .FirstOrDefaultAsync(b => b.BookingId == bookingId);
        }

        public async Task<IEnumerable<Booking>> GetBookingsByGuestIdAsync(int guestId)
        {
            return await _context.Bookings
                .Include(b => b.Room)
                .Include(b => b.SlotBookings) 
                .Where(b => b.Id == guestId) 
                .ToListAsync();
        }

        public async Task CreateBookingAsyncc(Booking booking)
        {
            await _context.Bookings.AddAsync(booking);
        }

        public async Task<Booking?> GetBookingByIdAsyncc(int bookingId)
        {
            return await _context.Bookings.FindAsync(bookingId);
        }


    }


}
