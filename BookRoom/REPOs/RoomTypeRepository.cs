using BOs.DTO;
using BOs.Entity;
using DAOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REPOs
{
    public class RoomTypeRepository : IRoomTypeRepository
    {
        private readonly IRoomTypeDAO _roomTypeDAO;
        private readonly BookingRoommContext _context;
        private readonly IRoomDAO _roomDAO;



        public RoomTypeRepository(IRoomTypeDAO roomTypeDAO, BookingRoommContext context, IRoomDAO roomDAO)
        {
            _roomTypeDAO = roomTypeDAO;
            _context = context;
            _roomDAO = roomDAO;
        }

        public async Task<IEnumerable<RoomType>> GetAllRoomTypes()
        {
            return await _roomTypeDAO.GetAllRoomTypes();
        }

        public async Task<RoomType?> GetRoomTypeById(int id)
        {
            return await _roomTypeDAO.GetRoomTypeById(id);
        }

        public async Task AddRoomType(RoomType roomType)
        {
            _context.RoomTypes.Add(roomType); // Không cần gán RoomTypeId
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<RoomType>> GetAllRoomTypesAsync()
        {
            return await _roomTypeDAO.GetAllRoomTypesAsync();
        }

        public async Task<RoomType?> GetRoomTypeByIdAsync(int roomTypeId)
        {
            return await _roomTypeDAO.GetRoomTypeByIdAsync(roomTypeId);
        }
        public async Task<bool> DeleteRoomTypeAsync(int roomTypeId)
        {
            return await _roomTypeDAO.DeleteRoomTypeAsync(roomTypeId);
        }
        public async Task<bool> UpdateRoomTypeAsync(int roomTypeId, UpdateRoomTypeDTO updateRoomTypeDTO)
        {
            return await _roomTypeDAO.UpdateRoomTypeAsync(roomTypeId, updateRoomTypeDTO);
        }
        public async Task<RoomType> CreateRoomTypeAsync(CreateRoomTypeDTO createRoomTypeDTO)
        {
            return await _roomTypeDAO.CreateRoomTypeAsync(createRoomTypeDTO);
        }
    }
}
