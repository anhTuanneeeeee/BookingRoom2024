using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using REPOs;

namespace BookingController.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly IStatusRepository _statusRepository;

        public StatusController(IStatusRepository statusRepository)
        {
            _statusRepository = statusRepository;
        }

        [HttpPost("initialize-statuses")]
        public async Task<IActionResult> InitializeStatuses()
        {
            var statuses = new[]
            {
                new { Id = 1, Name = "Slot Trống" },
                new { Id = 2, Name = "Slot đang được đặt" },
                new { Id = 3, Name = "Slot đã được đặt" },
                new { Id = 4, Name = "Payment Successful" },
                new { Id = 5, Name = "Payment Unsuccessful" }
            };

            foreach (var status in statuses)
            {
                await _statusRepository.CreateStatusAsync(status.Id, status.Name);
            }

            return Ok("Statuses initialized successfully.");
        }
        [HttpGet("GetAllStatus")]
        public async Task<IActionResult> GetAllStatus()
        {
            var statuses = await _statusRepository.GetAllStatusAsync();
            return Ok(statuses);
        }

        [HttpGet("GetStatusById")]
        public async Task<IActionResult> GetStatusById(int statusId)
        {
            var status = await _statusRepository.GetStatusByIdAsync(statusId);
            if (status == null)
            {
                return NotFound(new { Message = "Status not found" });
            }
            return Ok(status);
        }
    }
}
