using BOs.DTO;
using BOs.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using REPOs;

namespace BookingController.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BranchController : ControllerBase
    {
        private readonly IBranchRepository _branchRepository;

        public BranchController(IBranchRepository branchRepository)
        {
            _branchRepository = branchRepository;
        }
        [HttpPost]
        public async Task<IActionResult> CreateBranch([FromBody] BranchCreateDTO branchCreateDto)
        {
            if (branchCreateDto == null)
            {
                return BadRequest("Invalid branch data.");
            }

            var branch = new Branch
            {
                BranchName = branchCreateDto.BranchName,
                Location = branchCreateDto.Location,
                PhoneNumber = branchCreateDto.PhoneNumber
            };

            await _branchRepository.AddBranch(branch);
            return CreatedAtAction(nameof(GetBranchById), new { id = branch.BranchId }, branch);
        }

        [HttpGet("GetBranchById")]
        public async Task<ActionResult<BranchDTO>> GetBranchById(int id)
        {
            var branch = await _branchRepository.GetBranchById(id);
            if (branch == null) return NotFound();

            return Ok(branch);
        }
        [HttpGet("GetAllBranches")]
        public async Task<IActionResult> GetAllBranches()
        {
            var branches = await _branchRepository.GetAllBranches();
            return Ok(branches);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchBranches([FromQuery] string searchTerm)
        {
            var branches = await _branchRepository.SearchBranches(searchTerm);
            return Ok(branches);
        }

        [HttpPut("BranchById")]
        public async Task<IActionResult> UpdateBranch(int id, [FromBody] BranchUpdateDTO branchUpdateDto)
        {
            if (branchUpdateDto == null)
            {
                return BadRequest("Invalid branch data.");
            }

            var branch = await _branchRepository.GetBranchById(id);
            if (branch == null)
            {
                return NotFound($"Branch with ID {id} not found.");
            }

            // Cập nhật thông tin chi nhánh
            branch.BranchName = branchUpdateDto.BranchName;
            branch.Location = branchUpdateDto.Location;
            branch.PhoneNumber = branchUpdateDto.PhoneNumber;

            await _branchRepository.UpdateBranch(branch);
            return Ok("updated successfully"); // Trả về 204 No Content
        }

        [HttpDelete("BranchById")]
        public async Task<IActionResult> DeleteBranch(int id)
        {
            await _branchRepository.DeleteBranch(id);
            return Ok("Delete successfully");
        }
    }
}
