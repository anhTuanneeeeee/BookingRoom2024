using BOs.DTO;
using BOs.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using REPOs;

namespace BookingController.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomTypeController : ControllerBase
    {
        private readonly IRoomTypeRepository _roomTypeRepository;

        public RoomTypeController(IRoomTypeRepository roomTypeRepository)
        {
            _roomTypeRepository = roomTypeRepository;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateRoomType([FromBody] RoomTypeCreateDTO roomTypeDto)
        {
            if (string.IsNullOrEmpty(roomTypeDto.TypeName) || roomTypeDto.Price <= 0)
            {
                return BadRequest("Room type name and price are required.");
            }

            var roomType = new RoomType
            {
                TypeName = roomTypeDto.TypeName,
                Description = roomTypeDto.Description,
               
            };

            await _roomTypeRepository.AddRoomType(roomType);
            return Ok(roomType);
        }

        /*[HttpPost("seed")]
        public async Task<IActionResult> SeedDefaultRoomTypes()
        {
            var defaultRoomTypes = new List<RoomType>
            {
                new RoomType { TypeName = "Phòng đơn", Description = "Phòng dành cho 1 người", },
                new RoomType { TypeName = "Phòng đôi", Description = "Phòng dành cho 2 người",  },
                new RoomType { TypeName = "Phòng họp", Description = "Phòng dành cho họp",  }
            };

            foreach (var roomType in defaultRoomTypes)
            {
                await _roomTypeRepository.AddRoomType(roomType);
            }

            return Ok("Room types seeded successfully.");
        }*/
        [HttpGet("GetAllRoomType")]
        public async Task<ActionResult<IEnumerable<RoomType>>> GetAllRoomTypes()
        {
            var roomTypes = await _roomTypeRepository.GetAllRoomTypesAsync();
            return Ok(roomTypes);
        }

        // API lấy RoomType theo ID
        [HttpGet("GetRoomTypeById")]
        public async Task<ActionResult<RoomType>> GetRoomTypeById(int roomTypeId)
        {
            var roomType = await _roomTypeRepository.GetRoomTypeByIdAsync(roomTypeId);
            if (roomType == null)
            {
                return NotFound("Room type không tồn tại.");
            }
            return Ok(roomType);
        }
        [HttpDelete("roomTypeId")]
        public async Task<IActionResult> DeleteRoomType(int roomTypeId)
        {
            var deleted = await _roomTypeRepository.DeleteRoomTypeAsync(roomTypeId);
            if (!deleted)
            {
                return NotFound("RoomType không tồn tại.");
            }
            return Ok("RoomType đã được xóa thành công.");
        }
        [HttpPut("roomTypeId")]
        public async Task<IActionResult> UpdateRoomType(int roomTypeId, [FromBody] UpdateRoomTypeDTO updateRoomTypeDTO)
        {
            var updated = await _roomTypeRepository.UpdateRoomTypeAsync(roomTypeId, updateRoomTypeDTO);
            if (!updated)
            {
                return NotFound("RoomType không tồn tại.");
            }
            return Ok("RoomType đã được cập nhật thành công.");
        }
       
        [HttpPost("TaoRoomTypeTheoRomeID")]
        public async Task<IActionResult> CreateRoomType([FromBody] CreateRoomTypeDTO createRoomTypeDTO)
        {
            var roomType = await _roomTypeRepository.CreateRoomTypeAsync(createRoomTypeDTO);
            return CreatedAtAction(nameof(CreateRoomType), new { id = roomType.RoomTypeId }, roomType);
        }

    }


}
