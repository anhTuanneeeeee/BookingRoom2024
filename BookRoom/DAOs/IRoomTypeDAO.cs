using BOs.DTO;
using BOs.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAOs
{
    public interface IRoomTypeDAO
    {
        Task<RoomType?> GetRoomTypeById(int id);
        Task<IEnumerable<RoomType>> GetAllRoomTypes();
        Task AddRoomType(RoomType roomType);
        Task DeleteRoomType(int id);
        Task<IEnumerable<RoomType>> GetAllRoomTypesAsync();
        Task<RoomType?> GetRoomTypeByIdAsync(int roomTypeId);
        Task<bool> DeleteRoomTypeAsync(int roomTypeId);
        Task<bool> UpdateRoomTypeAsync(int roomTypeId, UpdateRoomTypeDTO updateRoomTypeDTO);
        Task<RoomType> CreateRoomTypeAsync(CreateRoomTypeDTO createRoomTypeDTO);
    }
}
