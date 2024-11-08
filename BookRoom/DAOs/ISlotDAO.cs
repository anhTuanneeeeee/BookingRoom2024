using BOs.DTO;
using BOs.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAOs
{
    public interface ISlotDAO
    {
        Task<Slot> CreateSlot(Slot slot);
       
        Task<Slot?> GetSlotById(int id);

        Task<IEnumerable<Slot>> GetSlotsByRoomId(int roomId);

        Task<IEnumerable<Slot>> GetSlotsByBranchId(int branchId);
        Task UpdateSlot(Slot slot);
        Task<bool> DeleteSlot(int id);

        Task CreateSlotsAsync(IEnumerable<Slot> slots);
        Task<bool> UpdateSlotStatusByIdAsync(int slotId, int statusId);
        Task<bool> CheckSlotsAvailabilityAsync(List<int> slotIds);
    }
}
