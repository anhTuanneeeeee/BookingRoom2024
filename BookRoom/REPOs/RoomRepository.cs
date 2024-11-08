using Azure.Core;
using BOs.DTO;
using BOs.Entity;
using DAOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REPOs
{
    public class RoomRepository : IRoomRepository
    {
        private readonly IRoomDAO _roomDAO;
        private readonly BookingRoommContext _context;

        public RoomRepository(IRoomDAO roomDAO, BookingRoommContext bookroomSwdContext)
        {
            _roomDAO = roomDAO;
            _context = bookroomSwdContext;
        }

        /*public async Task<Room> AddRoom(CreateRoomDTO createRoomDto)
        {
            var room = new Room
            {
                RoomName = createRoomDto.RoomName,
                RoomTypeId = createRoomDto.RoomTypeId,
                BranchId = createRoomDto.BranchId,
                IsAvailable = createRoomDto.IsAvailable,
                Price = createRoomDto.Price
            };

            _context.Rooms.Add(room);


            await _context.SaveChangesAsync();

            // Load the Branch and RoomType after saving
            await _context.Entry(room).Reference(r => r.Branch).LoadAsync();
            await _context.Entry(room).Reference(r => r.RoomType).LoadAsync();

            return room;
        }*/
        /*public async Task<Room> AddRoom(CreateRoomDTO createRoomDto)
        {
            var room = new Room
            {
                RoomName = createRoomDto.RoomName,
                RoomTypeId = createRoomDto.RoomTypeId,
                BranchId = createRoomDto.BranchId,
                IsAvailable = createRoomDto.IsAvailable,
                Price = createRoomDto.Price
            };

            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();

            // Tạo các khung giờ từ 7h sáng đến 21h
            var slots = new List<Slot>();
            TimeSpan startTime = new TimeSpan(7, 0, 0); 
            TimeSpan endTime = new TimeSpan(21, 0, 0); 
            TimeSpan interval = new TimeSpan(1, 0, 0); // Cách nhau 1 tiếng

            int defaultStatusId = 1;
            for (var time = startTime; time <= endTime; time = time.Add(interval))
            {
                var slot = new Slot
                {
                    RoomId = room.RoomId, 
                    StartTime = time.ToString(@"hh\:mm"), // Chuyển đổi TimeSpan thành chuỗi
                    EndTime = time.Add(interval).ToString(@"hh\:mm"), // Chuyển đổi TimeSpan thành chuỗi
                    StatusId = defaultStatusId,
                    
                };
                slots.Add(slot);
            }

            // Thêm các khung giờ vào database
            await _context.Slots.AddRangeAsync(slots);
            await _context.SaveChangesAsync();

            // Load the Branch and RoomType after saving
            await _context.Entry(room).Reference(r => r.Branch).LoadAsync();
            await _context.Entry(room).Reference(r => r.RoomType).LoadAsync();

            return room;
        }*/
        public async Task<Room> AddRoom(CreateRoomDTO createRoomDto)
        {
            var room = new Room
            {
                RoomName = createRoomDto.RoomName,
                RoomTypeId = createRoomDto.RoomTypeId,
                BranchId = createRoomDto.BranchId,
                IsAvailable = createRoomDto.IsAvailable,
                Price = createRoomDto.Price
            };

            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();

            // Tạo các slot từ 7 giờ sáng đến 21 giờ cho 7 ngày
            var slots = new List<Slot>();
            TimeSpan startTime = new TimeSpan(7, 0, 0);
            TimeSpan endTime = new TimeSpan(21, 0, 0);
            TimeSpan interval = new TimeSpan(1, 0, 0); // Mỗi slot cách nhau 1 tiếng

            int defaultStatusId = 1;

            // Lặp qua từng ngày trong 7 ngày tiếp theo, bao gồm cả ngày hôm nay
            for (int day = 0; day < 7; day++)
            {
                DateTime slotDate = DateTime.Today.AddDays(day); // Ngày hiện tại + số ngày

                for (var time = startTime; time < endTime; time = time.Add(interval))
                {
                    var slot = new Slot
                    {
                        RoomId = room.RoomId,
                        StartTime = slotDate.Add(time).ToString("yyyy-MM-dd HH:mm"), // Định dạng đầy đủ ngày và giờ
                        EndTime = slotDate.Add(time.Add(interval)).ToString("yyyy-MM-dd HH:mm"),
                        StatusId = defaultStatusId,
                    };
                    slots.Add(slot);
                }
            }

            // Thêm các slot vào database
            await _context.Slots.AddRangeAsync(slots);
            await _context.SaveChangesAsync();

            // Load the Branch and RoomType after saving
            await _context.Entry(room).Reference(r => r.Branch).LoadAsync();
            await _context.Entry(room).Reference(r => r.RoomType).LoadAsync();

            return room;
        }

        public async Task<GetRoomByIdDTO?> GetRoomById(int roomId)
        {
            return await _roomDAO.GetRoomById(roomId);
        }

        public async Task<IEnumerable<GetRoomDTO>> GetAllRooms()
        {
            return await _roomDAO.GetAllRooms();
        }

        public async Task<IEnumerable<GetRoomDTO>> GetRoomsByBranchId(int branchId)
        {
            return await _roomDAO.GetRoomsByBranchId(branchId);
        }
        public async Task<IEnumerable<Room>> GetRoomsByRoomTypeIdAsync(int roomTypeId)
        {
            return await _roomDAO.GetRoomsByRoomTypeIdAsync(roomTypeId);
        }

        public async Task DeleteRoom(int id)
        {
            await _roomDAO.DeleteRoom(id);
        }
        public async Task<bool> UpdateRoom(int roomId, UpdateRoomDTO updateRoomDTO)
        {
            return await _roomDAO.UpdateRoom(roomId, updateRoomDTO);
        }
    }

}
