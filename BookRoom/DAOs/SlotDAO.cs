using BOs.DTO;
using BOs.Entity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAOs
{
    public class SlotDAO : ISlotDAO
    {
        private readonly BookingRoommContext _context;

        public SlotDAO(BookingRoommContext context)
        {
            _context = context;
        }

        public async Task<Slot> CreateSlot(Slot slot)
        {
            _context.Slots.Add(slot);
            await _context.SaveChangesAsync();
            return await _context.Slots
                .Include(s => s.Room) 
                .FirstOrDefaultAsync(s => s.SlotId == slot.SlotId);
        }

        public async Task<IEnumerable<Slot>> GetSlotsByRoomId(int roomId)
        {
            return await _context.Slots
                .Where(s => s.RoomId == roomId)
                .Include(s => s.Room)
                .ThenInclude(r => r.Branch)
                .ToListAsync();
        }

        public async Task<Slot?> GetSlotById(int slotId)
        {
            return await _context.Slots
                .Include(s => s.Room)
                .ThenInclude(r => r.Branch) // Nạp dữ liệu Branch của Room
                .FirstOrDefaultAsync(s => s.SlotId == slotId);
        }

        public async Task<IEnumerable<Slot>> GetSlotsByBranchId(int branchId)
        {
            return await _context.Slots
                .Include(s => s.Room)
                .ThenInclude(r => r.Branch)
                .Where(s => s.Room.BranchId == branchId)
                .ToListAsync();
        }

        public async Task UpdateSlot(Slot slot)
        {
            _context.Slots.Update(slot);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteSlot(int id)
        {
            var slot = await GetSlotById(id);
            if (slot == null) return false;

            _context.Slots.Remove(slot);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task CreateSlotsAsync(IEnumerable<Slot> slots)
        {
            _context.Slots.AddRange(slots);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> UpdateSlotStatusByIdAsync(int slotId, int statusId)
        {
            var slot = await _context.Slots.FindAsync(slotId);
            if (slot == null) return false;

            slot.StatusId = statusId;
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> CheckSlotsAvailabilityAsync(List<int> slotIds)
        {
            return await _context.Slots
                .AnyAsync(slot => slotIds.Contains(slot.SlotId) && (slot.StatusId == 2 || slot.StatusId == 3));
        }

    }
}
