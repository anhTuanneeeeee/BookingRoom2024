using BOs.DTO;
using BOs.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REPOs
{
    public interface IRoomRepository
    {
        Task<Room> AddRoom(CreateRoomDTO createRoomDto);
        Task<GetRoomByIdDTO?> GetRoomById(int roomId);
        Task<IEnumerable<GetRoomDTO>> GetAllRooms();
        Task<IEnumerable<GetRoomDTO>> GetRoomsByBranchId(int branchId);
        Task DeleteRoom(int id);

        Task<bool> UpdateRoom(int roomId, UpdateRoomDTO updateRoomDTO);
        Task<IEnumerable<Room>> GetRoomsByRoomTypeIdAsync(int roomTypeId);
    }

}
