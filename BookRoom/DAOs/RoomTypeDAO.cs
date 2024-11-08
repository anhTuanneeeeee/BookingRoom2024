using BOs.DTO;
using BOs.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAOs
{
    public class RoomTypeDAO : IRoomTypeDAO
    {
        private readonly BookingRoommContext _context;

        public RoomTypeDAO(BookingRoommContext context)
        {
            _context = context;
        }

        public RoomTypeDAO()
        {
        }

        public async Task<RoomType?> GetRoomTypeById(int id)
        {
            return await _context.RoomTypes.FindAsync(id);
        }

        public async Task<IEnumerable<RoomType>> GetAllRoomTypes()
        {
            return await _context.RoomTypes.ToListAsync();
        }

        public async Task AddRoomType(RoomType roomType)
        {
            _context.RoomTypes.Add(roomType);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRoomType(int id)
        {
            var roomType = await _context.RoomTypes.FindAsync(id);
            if (roomType != null)
            {
                _context.RoomTypes.Remove(roomType);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<RoomType>> GetAllRoomTypesAsync()
        {
            return await _context.RoomTypes.ToListAsync();
        }

        public async Task<RoomType?> GetRoomTypeByIdAsync(int roomTypeId)
        {
            return await _context.RoomTypes.FindAsync(roomTypeId);
        }
        public async Task<bool> DeleteRoomTypeAsync(int roomTypeId)
        {
            var roomType = await _context.RoomTypes.FindAsync(roomTypeId);
            if (roomType == null)
            {
                return false; // Không tìm thấy RoomType
            }

            _context.RoomTypes.Remove(roomType);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateRoomTypeAsync(int roomTypeId, UpdateRoomTypeDTO updateRoomTypeDTO)
        {
            var roomType = await _context.RoomTypes.FindAsync(roomTypeId);
            if (roomType == null)
            {
                return false; // Không tìm thấy RoomType
            }

            roomType.TypeName = updateRoomTypeDTO.TypeName;
            roomType.Description = updateRoomTypeDTO.Description;
            
            roomType.Utilities = updateRoomTypeDTO.Utilities;

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<RoomType> CreateRoomTypeAsync(CreateRoomTypeDTO createRoomTypeDTO)
        {
            var roomType = new RoomType
            {
                TypeName = createRoomTypeDTO.TypeName,
                Description = createRoomTypeDTO.Description,
                
            };

            _context.RoomTypes.Add(roomType);
            await _context.SaveChangesAsync();

            return roomType;
        }
    }
}
