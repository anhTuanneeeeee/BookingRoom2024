
using BOs.DTO;
using BOs.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAOs
{
    public interface IRoomDAO
    {
        Task<Room> AddRoom(Room room);
        Task<GetRoomByIdDTO?> GetRoomById(int roomId);
        Task<IEnumerable<GetRoomDTO>> GetAllRooms();
        Task<IEnumerable<GetRoomDTO>> GetRoomsByBranchId(int branchId);
        Task DeleteRoom(int id);
        Task<Room?> GetRoomWithDetails(int id);

        
        Task<bool> UpdateRoom(int roomId, UpdateRoomDTO updateRoomDTO);
        Task<int> GetDefaultRoomIdAsync();
        Task<IEnumerable<Room>> GetRoomsByRoomTypeIdAsync(int roomTypeId);
    }
}
