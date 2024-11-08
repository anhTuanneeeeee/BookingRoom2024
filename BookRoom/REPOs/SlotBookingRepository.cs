using BOs.Entity;
using DAOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REPOs
{
    public class SlotBookingRepository : ISlotBookingRepository
    {
        private readonly ISlotBookingDAO _slotBookingDAO;

        public SlotBookingRepository(ISlotBookingDAO slotBookingDAO)
        {
            _slotBookingDAO = slotBookingDAO;
        }

        public async Task CreateSlotBookingAsync(SlotBooking slotBooking)
        {
            await _slotBookingDAO.CreateSlotBookingAsync(slotBooking);
        }

        public async Task<List<SlotBooking>> GetSlotBookingsByBookingIdAsync(int bookingId)
        {
            return await _slotBookingDAO.GetSlotBookingsByBookingIdAsync(bookingId);
        }
    }
}
