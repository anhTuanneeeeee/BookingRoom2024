using BOs.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookingController.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly BookingRoommContext _context;

        public RoleController(BookingRoommContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] Role role)
        {
            if (role == null || string.IsNullOrEmpty(role.RoleName))
            {
                return BadRequest("Invalid role data.");
            }

            _context.Roles.Add(role);
            await _context.SaveChangesAsync();

            return Ok(role);
        }
    }
}
