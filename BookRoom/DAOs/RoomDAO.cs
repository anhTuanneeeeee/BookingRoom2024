using BOs.DTO;
using BOs.Entity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DAOs
{
    public class RoomDAO : IRoomDAO
    {
        private readonly BookingRoommContext _context;

        public RoomDAO(BookingRoommContext context)
        {
            _context = context;
        }

        public async Task<Room> AddRoom(Room room)
        {
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();
            return room;
        }
        public async Task<int> GetDefaultRoomIdAsync()
{
    
    var defaultRoom = await _context.Rooms.FirstOrDefaultAsync(); // Lấy phòng đầu tiên
    return defaultRoom?.RoomId ?? 0; // Trả về RoomId hoặc 0 nếu không có
}
        public async Task<Room?> GetRoomWithDetails(int id)
        {
            return await _context.Rooms
                .Include(r => r.Branch) 
                .Include(r => r.RoomType) 
                .FirstOrDefaultAsync(r => r.RoomId == id);
        }

        public async Task<GetRoomByIdDTO> GetRoomById(int roomId)
        {
            var room = await _context.Rooms
                .Where(r => r.RoomId == roomId)
                .Include(r => r.Branch)
                .Include(r => r.RoomType)
                .Select(r => new GetRoomByIdDTO
                {
                    RoomId = r.RoomId,
                    RoomName = r.RoomName,
                    RoomTypeId = r.RoomTypeId ?? 0,
                    RoomTypeName = r.RoomType.TypeName,  // Include RoomTypeName
                    BranchId = r.BranchId ?? 0,
                    BranchName = r.Branch.BranchName,  // Include BranchName
                    Price = r.Price ?? 0,
                })
                .FirstOrDefaultAsync();

            return room;
        }

        public async Task<IEnumerable<GetRoomDTO>> GetAllRooms()
        {
            return await _context.Rooms
                .Include(r => r.Branch)
                .Include(r => r.RoomType)
                .Select(r => new GetRoomDTO
                {
                    RoomId = r.RoomId,
                    RoomName = r.RoomName,
                    BranchName = r.Branch.BranchName,
                    RoomTypeId=r.RoomTypeId ?? 0,
                    RoomTypeName = r.RoomType.TypeName,
                    IsAvailable = r.IsAvailable ?? false, 
                    Price=r.Price ?? 0,
                })
                .ToListAsync();
        }
        public async Task<IEnumerable<Room>> GetRoomsByRoomTypeIdAsync(int roomTypeId)
        {
            return await _context.Rooms
                .Where(r => r.RoomTypeId == roomTypeId)
                .ToListAsync();
        }

        public async Task<IEnumerable<GetRoomDTO>> GetRoomsByBranchId(int branchId)
        {
            var rooms = await _context.Rooms
                .Where(r => r.BranchId == branchId)
                .Include(r => r.Branch) 
                .Include(r => r.RoomType) 
                .ToListAsync();

            // Trả về danh sách DTO với các thông tin cần thiết
            return rooms.Select(room => new GetRoomDTO
            {
                RoomId = room.RoomId,
                RoomName = room.RoomName,
                BranchName = room.Branch.BranchName, // Lấy tên chi nhánh
                RoomTypeName = room.RoomType.TypeName, // Lấy tên loại phòng
                IsAvailable = room.IsAvailable ?? false,
                Price = room.Price ?? 0,
               
            });
        }

        public async Task DeleteRoom(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room != null)
            {
                _context.Rooms.Remove(room);
                await _context.SaveChangesAsync();
            }
        }

       
        public async Task<bool> UpdateRoom(int roomId, UpdateRoomDTO updateRoomDTO)
        {
            var room = await _context.Rooms.FindAsync(roomId);
            if (room == null) return false; // Không tìm thấy phòng

            room.RoomName = updateRoomDTO.RoomName;
            room.RoomTypeId = updateRoomDTO.RoomTypeId;
            room.BranchId = updateRoomDTO.BranchId;
            room.Price = updateRoomDTO.Price;

            await _context.SaveChangesAsync(); // Lưu thay đổi vào cơ sở dữ liệu
            return true;
        }
    }

}
