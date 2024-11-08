using BOs.DTO;
using BOs.Entity;
using DAOs;
using Microsoft.EntityFrameworkCore;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REPOs
{
    public class BookingRepository : IBookingRepository
    {   
        private readonly BookingRoommContext _context;
        private readonly IBookingDAO _bookingDAO;
        private readonly IRoomDAO _roomDAO; // Để lấy thông tin phòng
        private readonly ISlotDAO _slotDAO; // Để lấy thông tin slot
        private readonly ISlotBookingDAO _slotBookingDAO;

        public BookingRepository(IBookingDAO bookingDAO, IRoomDAO roomDAO, ISlotDAO slotDAO, BookingRoommContext context, ISlotBookingDAO slotBookingDAO)
        {
            _bookingDAO = bookingDAO;
            _roomDAO = roomDAO;
            _slotDAO = slotDAO;
            _context = context;
            _slotBookingDAO = slotBookingDAO;
        }

        public async Task<Booking> BookRoomAsync(int roomId, int slotId, int customerId)
        {
            // Tìm thông tin phòng dựa trên RoomId
            var room = await _context.Rooms.FindAsync(roomId);
            if (room == null)
            {
                throw new Exception("Room not found");
            }
            var slot = await _context.Slots.Include(s => s.Bookings) 
                                      .FirstOrDefaultAsync(s => s.SlotId == slotId);
            if (slot == null)
            {
                throw new Exception("Slot not found");
            }
            if (slot.StatusId != 1) 
            {
                throw new Exception("This slot is currently being reserved or has already been booked.");
            }


            // Cập nhật trạng thái của slot thành "Slot Being Reserved"
            slot.StatusId = 2; 
            await _context.SaveChangesAsync();
            // Tạo đối tượng Booking
            var booking = new Booking
            {
                RoomId = roomId,
                SlotId = slotId,
                Id = customerId, 
                BookingDate = DateTime.Now, // Thời gian booking
                CreatedAt = DateTime.Now,
                TotalFee = room.Price // Lưu giá phòng vào TotalFee
            };

            return await _bookingDAO.CreateBookingAsync(booking); 
        }
        public async Task<Booking> CreateBookingAsync(CreateBookingDTO bookingRequest)
        {
            // Kiểm tra xem các slot có đang được đặt không
            var slots = await _context.Slots.Where(s => bookingRequest.SlotIds.Contains(s.SlotId)).ToListAsync();

            // Kiểm tra trạng thái của từng slot
            foreach (var slot in slots)
            {
                if (slot.StatusId == 2 || slot.StatusId == 3) // Slot đang được đặt hoặc đã được đặt
                {
                    throw new InvalidOperationException("One or more slots are already booked.");
                }
            }

            // Lấy thông tin phòng để lấy giá
            var room = await _context.Rooms.FindAsync(bookingRequest.RoomId);
            if (room == null)
            {
                throw new InvalidOperationException("Room not found.");
            }

            // Tiến hành tạo booking
            var booking = new Booking
            {
                RoomId = bookingRequest.RoomId,
                Id = bookingRequest.CustomerId,
                BookingDate = DateTime.UtcNow, // Đặt thời gian đặt phòng hiện tại
            };

            // Tính toán tổng phí
            decimal totalFee = (decimal)(room.Price * bookingRequest.SlotIds.Count); // Giá phòng nhân với số lượng slot được đặt
            booking.TotalFee = totalFee; // Gán tổng phí cho booking

            // Cập nhật trạng thái slot
            foreach (var slot in slots)
            {
                slot.StatusId = 2; // Cập nhật trạng thái thành "Đang được đặt"
            }

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            // Ghi lại các slot được đặt
            foreach (var slotId in bookingRequest.SlotIds)
            {
                var slotBooking = new SlotBooking
                {
                    BookingId = booking.BookingId,
                    SlotId = slotId
                };
                _context.SlotBookings.Add(slotBooking);
            }

            await _context.SaveChangesAsync();

            return booking;
        }
        public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
        {
            return await _bookingDAO.GetAllBookingsAsync();
        }

        public async Task<Booking?> GetBookingByIdAsync(int bookingId)
        {
            return await _bookingDAO.GetBookingByIdAsync(bookingId);
        }

        public async Task<IEnumerable<Booking>> GetBookingsByGuestIdAsync(int guestId)
        {
            return await _bookingDAO.GetBookingsByGuestIdAsync(guestId);
        }

        public async Task CreateBookingAsyncc(CreateBookingDTO createBookingDto)
        {
            // Chuyển đổi từ CreateBookingDTO sang Booking
            var booking = new Booking
            {
                Id = createBookingDto.CustomerId,
                RoomId = createBookingDto.RoomId,
                CreatedAt = DateTime.UtcNow,
                BookingDate = DateTime.UtcNow,
                TotalFee = 0 // Tính toán phí sau này
            };

            // Lưu vào cơ sở dữ liệu
            await _bookingDAO.CreateBookingAsync(booking);

            // Lưu các SlotBookings cho các slot được đặt
            foreach (var slotId in createBookingDto.SlotIds)
            {
                var slotBooking = new SlotBooking
                {
                    BookingId = booking.BookingId, // Giả sử bạn có BookingId sau khi lưu booking
                    SlotId = slotId,
                    StatusId = 1 // Ví dụ, bạn có thể muốn đặt status là 'Slot Available'
                };

                // Thêm vào danh sách slot bookings
                await _slotBookingDAO.CreateSlotBookingAsync(slotBooking);
            }
        }

        public async Task<Booking?> GetBookingByIdAsyncc(int bookingId)
        {
            return await _bookingDAO.GetBookingByIdAsync(bookingId);
        }

        public async Task<Booking> BookMultipleSlotsAsync(int roomId, List<int> slotIds, int customerId)
        {
            // Tìm thông tin phòng dựa trên RoomId
            var room = await _context.Rooms.FindAsync(roomId);
            if (room == null)
            {
                throw new Exception("Room not found");
            }

            decimal totalFee = 0;
            var bookings = new List<SlotBooking>();

            foreach (var slotId in slotIds)
            {
                var slot = await _context.Slots.Include(s => s.Bookings)
                                               .FirstOrDefaultAsync(s => s.SlotId == slotId && s.RoomId == roomId);

                if (slot == null)
                {
                    throw new Exception($"Slot with ID {slotId} not found in Room {roomId}");
                }

                if (slot.StatusId != 1) // Kiểm tra trạng thái slot có sẵn hay không
                {
                    throw new Exception($"Slot {slotId} is currently being reserved or has already been booked.");
                }

                // Cập nhật trạng thái của slot thành "Slot Being Reserved"
                slot.StatusId = 2;

                // Tính tổng phí cho các slot được book
                totalFee += room.Price ?? 0;

                // Thêm slot vào danh sách bookings
                bookings.Add(new SlotBooking
                {
                    SlotId = slotId,
                    StatusId = 2 // Slot đang được giữ chỗ
                });
            }

            await _context.SaveChangesAsync();

            // Tạo đối tượng Booking với thông tin nhiều slot
            var booking = new Booking
            {
                RoomId = roomId,
                Id = customerId,
                BookingDate = DateTime.Now,
                CreatedAt = DateTime.Now,
                TotalFee = totalFee
            };

            // Lưu booking vào database
            var createdBooking = await _bookingDAO.CreateBookingAsync(booking);

            // Thêm SlotBooking cho từng slot đã book
            foreach (var slotBooking in bookings)
            {
                slotBooking.BookingId = createdBooking.BookingId;
                _context.SlotBookings.Add(slotBooking);
            }

            await _context.SaveChangesAsync();

            return createdBooking;
        }
    }

}




