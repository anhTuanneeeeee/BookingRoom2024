using BOs.DTO;
using BOs.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using REPOs;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using SystemTextJson = System.Text.Json.JsonSerializer;

namespace BookingController.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly ISlotRepository _slotRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly ISlotBookingRepository _slotBookingRepository;
        private readonly BookingRoommContext _context;

        public BookingController(IBookingRepository bookingRepository, ISlotRepository slotRepository, IRoomRepository roomRepository, ISlotBookingRepository slotBookingRepository, BookingRoommContext context)
        {
            _bookingRepository = bookingRepository;
            _slotRepository = slotRepository;
            _roomRepository = roomRepository;
            _slotBookingRepository = slotBookingRepository;
            _context = context;
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateBooking([FromBody] BookingDTO bookingDto)
        {
            if (bookingDto == null)
            {
                return BadRequest("Invalid booking data.");
            }

            var booking = await _bookingRepository.BookRoomAsync(bookingDto.RoomId, bookingDto.SlotId, bookingDto.CustomerId);

            return CreatedAtAction(nameof(CreateBooking), new { id = booking.BookingId }, booking);
        }

        /*[HttpPost("create-booking")]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingDTO bookingRequest)
        {
            try
            {
                var booking = await _bookingRepository.CreateBookingAsync(bookingRequest);

                // Trả về thông tin booking với các trường cần thiết
                var response = new
                {
                    BookingId = booking.BookingId,
                    CustomerId = booking.Id,
                    RoomId = booking.RoomId,
                    TotalFee = booking.TotalFee,
                    SlotIds = bookingRequest.SlotIds // Danh sách SlotIds
                };

                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                // Ghi log chi tiết lỗi ở đây
                return StatusCode(500, new { Message = "Failed to create booking", Error = ex.Message });
            }
        }*/
        [HttpGet("all")]
        public async Task<IActionResult> GetAllBookings()
        {
            var bookings = await _bookingRepository.GetAllBookingsAsync();
            return Ok(bookings);
        }

        [HttpGet("{bookingId}")]
        public async Task<IActionResult> GetBookingById(int bookingId)
        {
            var booking = await _bookingRepository.GetBookingByIdAsync(bookingId);
            if (booking == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Status = 404,
                    StatusText = "Not Found",
                    Data = new { message = "Booking not found" }
                });
            }
            return Ok(booking);
        }

        [HttpGet("guest/{guestId}")]
        public async Task<IActionResult> GetBookingsByGuestId(int guestId)
        {
            var bookings = await _bookingRepository.GetBookingsByGuestIdAsync(guestId);
            return Ok(bookings);
        }

        /* [HttpPost("create_Booking_ANHTUAN")]
         public async Task<IActionResult> CreateBookingg([FromBody] BookingRequest bookingRequest)
         {
             var room = await _roomRepository.GetRoomById(bookingRequest.RoomId);
             if (room == null)
             {
                 return NotFound("Room not found");
             }

             var slots = await _slotRepository.GetSlotsByIdsAsync(bookingRequest.SlotIds);
             if (slots.Count != bookingRequest.SlotIds.Count)
             {
                 return BadRequest("One or more slots not found");
             }

             var unavailableSlots = slots.Where(slot => slot.StatusId != 1).ToList(); // Giả định 1 là Slot Available
             if (unavailableSlots.Any())
             {
                 return BadRequest("One or more slots are not available for booking");
             }

             var booking = new Booking
             {
                 Id = bookingRequest.CustomerId,
                 RoomId = bookingRequest.RoomId,
                 CreatedAt = DateTime.UtcNow,
                 BookingDate = DateTime.UtcNow,
                 TotalFee = room.Price * bookingRequest.SlotIds.Count
             };

             await _bookingRepository.CreateBookingAsync(Booking);
             await _context.SaveChangesAsync();

             foreach (var slot in slots)
             {
                 slot.StatusId = 2; // Đánh dấu slot là đang được đặt
                 var slotBooking = new SlotBooking
                 {
                     SlotId = slot.SlotId,
                     BookingId = booking.BookingId,
                     StatusId = 2 // Đánh dấu slot đã được đặt
                 };

                 await _slotBookingRepository.CreateSlotBookingAsync(slotBooking);
             }

             await _context.SaveChangesAsync();

             return Ok(new
             {
                 Message = "Booking created successfully",
                 Booking = booking
             });
         }*/

        /* [HttpPost("createe")]
         public async Task<IActionResult> CreateBookingg([FromBody] CreateBookingDTO createBookingDto)
         {
             // Kiểm tra tính hợp lệ của dữ liệu đầu vào
             if (!ModelState.IsValid)
             {
                 return BadRequest(ModelState);
             }

             // Gọi phương thức tạo booking
             await _bookingRepository.CreateBookingAsync(createBookingDto);

             return Ok(new { Message = "Booking created successfully" });
         }*/
        [HttpPost("BookingSLot")]
        public async Task<IActionResult> BookingNhieuSLot([FromBody] BookingNhieuSLotDTO bookingDto)
        {
            if (bookingDto == null || bookingDto.SlotIds == null || !bookingDto.SlotIds.Any())
            {
                return BadRequest("Invalid booking data.");
            }

            try
            {
                var booking = await _bookingRepository.BookMultipleSlotsAsync(
                    bookingDto.RoomId,
                    bookingDto.SlotIds,
                    bookingDto.CustomerId
                );

                return CreatedAtAction(nameof(BookingNhieuSLot), new { id = booking.BookingId }, booking);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }









    }
}
