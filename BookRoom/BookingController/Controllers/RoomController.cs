using BOs.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using REPOs;

namespace BookingController.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly IRoomRepository _roomRepository;

        public RoomController(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        [HttpPost("CrateRoom")]
        public async Task<IActionResult> CreateRoom([FromBody] CreateRoomDTO createRoomDto)
        {
            if (createRoomDto == null || string.IsNullOrWhiteSpace(createRoomDto.RoomName))
            {
                return BadRequest("Room data is invalid.");
            }

            var createdRoom = await _roomRepository.AddRoom(createRoomDto);
            return CreatedAtAction(nameof(GetRoomById), new { id = createdRoom.RoomId }, createdRoom);
        }



        [HttpGet("GetRoomById")]
        public async Task<IActionResult> GetRoomById(int roomId)
        {
            var room = await _roomRepository.GetRoomById(roomId);
            if (room == null)
            {
                return NotFound();
            }

            return Ok(room);
        }

        [HttpGet("GetAllRoom")]
        public async Task<IActionResult> GetAllRooms()
        {
            var rooms = await _roomRepository.GetAllRooms();
            return Ok(rooms);
        }

        [HttpGet("GetRoomsByBranchId")]
        public async Task<IActionResult> GetRoomsByBranchId(int branchId)
        {
            var rooms = await _roomRepository.GetRoomsByBranchId(branchId);
            return Ok(rooms);
        }

        /*[HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoom(int id, [FromBody] UpdateRoomDTO roomUpdateDTO)
        {
            var result = await _roomRepository.UpdateRoom(id, roomUpdateDTO);
            if (!result) return NotFound();

            return NoContent();
        }*/
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoom(int id, [FromBody] UpdateRoomDTO updateRoomDTO)
        {
            if (updateRoomDTO == null)
            {
                return BadRequest("Invalid room data.");
            }

            var result = await _roomRepository.UpdateRoom(id, updateRoomDTO);
            if (!result)
            {
                return NotFound(); // Phòng không tồn tại
            }

            return Ok("Room updated successfully."); // Cập nhật thành công, không trả về nội dung
        }
        [HttpGet("GetByRoomTypeId/{roomTypeId}")]
        public async Task<IActionResult> GetRoomsByRoomTypeId(int roomTypeId)
        {
            var rooms = await _roomRepository.GetRoomsByRoomTypeIdAsync(roomTypeId);

            if (rooms == null || !rooms.Any())
            {
                return NotFound(new ApiResponse<object>
                {
                    Status = 404,
                    StatusText = "Not Found",
                    Data = new { message = "No rooms found for the specified RoomTypeId" }
                });
            }

            return Ok(rooms);
        }




        [HttpDelete("DeleteRoom")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            await _roomRepository.DeleteRoom(id);
            return Ok("DeleRoom successfully");
        }
    }

}
