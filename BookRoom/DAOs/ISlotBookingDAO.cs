using BOs.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAOs
{
    public interface ISlotBookingDAO
    {
        Task CreateSlotBookingAsync(SlotBooking slotBooking);
        Task<List<SlotBooking>> GetSlotBookingsByBookingIdAsync(int bookingId);
    }
}
