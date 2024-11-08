using BOs.DTO;
using BOs.Entity;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REPOs
{
    public interface IBookingRepository
    {
        Task<Booking> BookRoomAsync(int roomId, int slotId, int customerId);
        Task<IEnumerable<Booking>> GetAllBookingsAsync();
        Task<Booking?> GetBookingByIdAsync(int bookingId);
        Task<IEnumerable<Booking>> GetBookingsByGuestIdAsync(int guestId);
        Task<Booking> CreateBookingAsync(CreateBookingDTO createBookingDto);
        
        Task<Booking?> GetBookingByIdAsyncc(int bookingId);
        Task<Booking> BookMultipleSlotsAsync(int roomId, List<int> slotIds, int customerId);


    }
}
