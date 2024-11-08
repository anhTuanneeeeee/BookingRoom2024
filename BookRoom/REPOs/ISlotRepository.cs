using BOs.DTO;
using BOs.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace REPOs
{
    public interface ISlotRepository
    {
        Task<Slot> CreateSlot(Slot slot);
        Task<IEnumerable<Slot>> GetSlotsByRoomId(int roomId);
        Task<IEnumerable<Slot>> GetSlotsByBranchId(int branchId);
        Task<Slot?> GetSlotById(int id);

        Task<bool> UpdateSlot(int slotId, UpdateSlotDTO updateSlotDTO);
        Task<bool> DeleteSlot(int id);
        Task<bool> UpdateSlotStatusByIdAsync(int slotId, int statusId);
        Task<bool> CheckSlotsAvailabilityAsync(List<int> slotIds);
        Task UpdateSlotStatusAsync(int slotId, int status);
        Task<List<Slot>> GetSlotsByIdsAsync(List<int> slotIds);
        Task UpdateSlotStatusByBookingId(int bookingId, int statusId);
    }
}
