using BOs.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAOs
{
    public interface IBookingDAO
    {
        Task<Booking> CreateBookingAsync(Booking booking);
        Task<IEnumerable<Booking>> GetAllBookingsAsync();
        Task<Booking?> GetBookingByIdAsync(int bookingId);
        Task<IEnumerable<Booking>> GetBookingsByGuestIdAsync(int guestId);

        Task CreateBookingAsyncc(Booking booking);
        Task<Booking?> GetBookingByIdAsyncc(int bookingId);
        


    }
}
