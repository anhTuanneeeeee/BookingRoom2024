using BOs.DTO;
using BOs.Entity;
using DAOs;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace REPOs
{
    public class SlotRepository : ISlotRepository
    {
        private readonly ISlotDAO _slotDAO;
        private readonly BookingRoommContext _context;

        public SlotRepository(ISlotDAO slotDAO, BookingRoommContext context)
        {
            _slotDAO = slotDAO;
            _context = context;
        }

        public async Task<Slot> CreateSlot(Slot slot)
        {
            return await _slotDAO.CreateSlot(slot);
        }

        public async Task<IEnumerable<Slot>> GetSlotsByRoomId(int roomId)
        {
            return await _slotDAO.GetSlotsByRoomId(roomId);
        }
        public async Task<IEnumerable<Slot>> GetSlotsByBranchId(int branchId)
        {
            return await _slotDAO.GetSlotsByBranchId(branchId);
        }

        public async Task<Slot?> GetSlotById(int id)
        {
            return await _slotDAO.GetSlotById(id);
        }
        public async Task<bool> UpdateSlot(int slotId, UpdateSlotDTO updateSlotDTO)
        {
            var slot = await _slotDAO.GetSlotById(slotId);
            if (slot == null) return false;

            slot.StartTime = updateSlotDTO.StartTime;
            slot.EndTime = updateSlotDTO.EndTime;
            slot.RoomId = updateSlotDTO.RoomId;

            await _slotDAO.UpdateSlot(slot);
            return true;
        }

        public async Task<bool> DeleteSlot(int id)
        {
            return await _slotDAO.DeleteSlot(id);
        }
        public async Task<bool> UpdateSlotStatusByIdAsync(int slotId, int statusId)
        {
            return await _slotDAO.UpdateSlotStatusByIdAsync(slotId, statusId);
        }
        public async Task<bool> CheckSlotsAvailabilityAsync(List<int> slotIds)
        {
            return await _slotDAO.CheckSlotsAvailabilityAsync(slotIds);
        }
        public async Task UpdateSlotStatusAsync(int slotId, int status)
        {
            var slot = await _context.Slots.FindAsync(slotId);
            if (slot != null)
            {
                slot.StatusId = status;
                await _context.SaveChangesAsync();
            }
        }
        public async Task<List<Slot>> GetSlotsByIdsAsync(List<int> slotIds)
        {
            return await _context.Slots
                .Where(slot => slotIds.Contains(slot.SlotId))
                .ToListAsync();
        }
        public async Task UpdateSlotStatusByBookingId(int bookingId, int statusId)
        {
            var slots = await _context.SlotBookings
                                      .Where(sb => sb.BookingId == bookingId)
                                      .ToListAsync();

            if (!slots.Any())
            {
                Console.WriteLine("No slots found for BookingId: " + bookingId);
                return;
            }
            foreach (var slot in slots)
            {
                slot.StatusId = statusId;
            }

            await _context.SaveChangesAsync();
        }
    }
}
