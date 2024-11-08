using BOs.DTO;
using BOs.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using REPOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookingController.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SlotController : ControllerBase
    {
        private readonly ISlotRepository _slotRepository;

        public SlotController(ISlotRepository slotRepository)
        {
            _slotRepository = slotRepository;
        }

        [HttpPost("CreateSlot")]
        public async Task<IActionResult> CreateSlot([FromBody] CreateSlotDTO slotDTO)
        {
            var slot = new Slot
            {
                RoomId = slotDTO.RoomId,
                StartTime = slotDTO.StartTime,
                EndTime = slotDTO.EndTime

            };

            var createdSlot = await _slotRepository.CreateSlot(slot);

            if (createdSlot == null)
            {
                return BadRequest("Unable to create slot.");
            }

            // Đảm bảo trả về dữ liệu Room đã nạp
            return Ok(new
            {
                createdSlot.SlotId,
                createdSlot.RoomId,
                createdSlot.StartTime,
                createdSlot.EndTime,
                createdSlot.StatusId,
                Room = new
                {
                    createdSlot.Room?.RoomId,
                    createdSlot.Room?.RoomName
                }
            });
        }
        [HttpGet("GetSlotById/{slotId}")]
        public async Task<IActionResult> GetSlotById(int slotId)
        {
            var slot = await _slotRepository.GetSlotById(slotId);
            if (slot == null)
            {
                return NotFound("Slot not found.");
            }

            return Ok(new
            {
                slot.SlotId,
                slot.RoomId,
                slot.StartTime,
                slot.EndTime,
                slot.StatusId,
              
                Room = new
                {
                    slot.Room?.RoomId,
                    slot.Room?.RoomName,
                    slot.Room?.Branch?.BranchName,
                    slot.Room?.Price
                }
            });
        }


        [HttpGet("GetSlotsByRoomId/{roomId}")]
        public async Task<IActionResult> GetSlotsByRoomId(int roomId)
        {
            var slots = await _slotRepository.GetSlotsByRoomId(roomId);
            if (!slots.Any())
            {
                return NotFound("No slots found for this Room.");
            }

            return Ok(slots.Select(slot => new
            {
                slot.SlotId,
                slot.RoomId,
                slot.StartTime,
                slot.EndTime,
                slot.StatusId,
                Room = new
                {
                    slot.Room?.RoomName,
                    slot.Room?.Branch?.BranchName,
                    slot.Room.Price
                }
            }));
        }

        [HttpGet("GetSlotsByBranchId/{branchId}")]
        public async Task<IActionResult> GetSlotsByBranchId(int branchId)
        {
            var slots = await _slotRepository.GetSlotsByBranchId(branchId);
            if (!slots.Any())
            {
                return NotFound("No slots found for this Branch.");
            }

            return Ok(slots.Select(slot => new
            {
                slot.SlotId,
                slot.RoomId,
                slot.StartTime,
                slot.EndTime,
                slot.StatusId,
                Room = new
                {
                    slot.Room?.RoomName,
                    Branch = slot.Room?.Branch?.BranchName,
                    slot.Room?.Price
                }
            }));
        }

        [HttpPut("{slotId}")]
        public async Task<IActionResult> UpdateSlot(int slotId, [FromBody] UpdateSlotDTO updateSlotDTO)
        {
            var updated = await _slotRepository.UpdateSlot(slotId, updateSlotDTO);
            if (!updated)
            {
                return NotFound("Slot not found.");
            }
            return Ok("Slot updated successfully.");
        }

        [HttpDelete("DeleteSlotById")]
        public async Task<IActionResult> DeleteSlot(int id)
        {
            var result = await _slotRepository.DeleteSlot(id);
            if (!result)
            {
                return NotFound("Slot not found.");
            }
            return NoContent(); // 204 No Content
        }

        [HttpPut("update-status/{slotId}")]
        public async Task<IActionResult> UpdateSlotStatus(int slotId, [FromBody] int statusId)
        {
            var result = await _slotRepository.UpdateSlotStatusByIdAsync(slotId, statusId);
            if (result)
            {
                return Ok(new { Message = "Slot status updated successfully." });
            }
            return NotFound(new { Message = "Slot not found." });
        }
    }
}
