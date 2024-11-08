using BOs.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAOs
{
    public class SlotBookingDAO : ISlotBookingDAO
    {
        private readonly BookingRoommContext _context;

        public SlotBookingDAO(BookingRoommContext context)
        {
            _context = context;
        }

        public async Task CreateSlotBookingAsync(SlotBooking slotBooking)
        {
            await _context.SlotBookings.AddAsync(slotBooking);
        }

        public async Task<List<SlotBooking>> GetSlotBookingsByBookingIdAsync(int bookingId)
        {
            return await _context.SlotBookings
                .Where(sb => sb.BookingId == bookingId)
                .ToListAsync();
        }
    }
}
