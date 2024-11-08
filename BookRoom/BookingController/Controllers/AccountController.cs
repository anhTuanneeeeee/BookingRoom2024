using BOs.DTO;
using BOs.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using REPOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookingController.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly BookingRoommContext _context;
        private readonly IJWTService _jWTService;

        public AccountController(IAccountRepository accountRepo, BookingRoommContext context, IJWTService jWTService)
        {
            _accountRepository = accountRepo;
            _context = context;
            _jWTService = jWTService;
        }

        [HttpPost("login")]
        public IActionResult Login(AccountDTO accountDTO)
        {
            var user = _accountRepository.GetAccount(accountDTO.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(accountDTO.Password, user.Password))
            {
                return BadRequest(new ApiResponse<object>
                {
                    Status = 400,
                    StatusText = "Bad Request",
                    Data = new { message = "Invalid username or password" }
                });
            }

            var role = _context.Roles.FirstOrDefault(r => r.RoleId == user.RoleId);
            if (role == null)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Status = 400,
                    StatusText = "Bad Request",
                    Data = new { message = "User role not found" }
                });
            }
            ///Dong Hai # Save value originalPassword  to use method view profile
            ///
            Response.Cookies.Append("originalPassword", accountDTO.Password);
            ///end
            ///

            var jwt = _jWTService.Generate(user.Id, role.RoleName);

            var userDto = new
            {

                userId = user.Id,
                accessToken = jwt
            };

            return Ok(new ApiResponse<object>
            {
                Status = 200,
                StatusText = "OK",
                Data = userDto
            });
        }
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO changePasswordDTO)
        {
            var result = await _accountRepository.ChangePasswordAsync(changePasswordDTO);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Message);
        }
    }
}
