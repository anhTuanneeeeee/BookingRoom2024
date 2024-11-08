using BOs.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using REPOs;

namespace BookingController.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly IGuestRepository _guestRepository;

        public StaffController(IGuestRepository guestRepository)
        {
            _guestRepository = guestRepository;
        }
        [HttpPost("createStaff")]
        public async Task<IActionResult> CreateStaff([FromBody] StaffCreateDTO staffDto)
        {
            if (string.IsNullOrEmpty(staffDto.UserName) ||
                string.IsNullOrEmpty(staffDto.Email) ||
                string.IsNullOrEmpty(staffDto.Password) ||
                string.IsNullOrEmpty(staffDto.PhoneNumber))
            {
                return BadRequest("All fields are required.");
            }
            try
            {
                var staff = await _guestRepository.CreateStaff(
                staffDto.UserName, staffDto.Email, staffDto.Password, staffDto.PhoneNumber
            );
                if (staff == null)
                {
                    return Conflict("Email already exists.");
                }

                return Ok(staff);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Email already exists"))
                {
                    return Conflict("Email already exists.");
                }
                if (ex.Message.Contains("Phone number already exists"))
                {
                    return Conflict("Phone number already exists.");
                }
                return StatusCode(500, "An error occurred while creating the Staff.");
            }
        }

        [HttpGet("GetAllStaff")]
        public async Task<ActionResult<IEnumerable<StaffDTO>>> GetAllStaff()
        {
            var staffList = await _guestRepository.GetAllStaffAsync();
            return Ok(staffList);
        }

        [HttpGet("GetStaffById/{id}")]
        public async Task<ActionResult<StaffDTO>> GetStaffById(int id)
        {
            var staff = await _guestRepository.GetStaffByIdAsync(id);

            if (staff == null)
            {
                return NotFound();
            }

            return Ok(staff);
        }

    }
}
