using BOs.Entity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using REPOs;
using System.Security.Claims;


namespace BookingController.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuestController : ControllerBase
    {
        private readonly IGuestRepository _guestRepository;

        public GuestController(IGuestRepository guestRepository)
        {
            _guestRepository = guestRepository;
        }

       
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("login-google")]
        public IActionResult LoginWithGoogle()
        {
            var redirectUrl = Url.Action("GoogleResponse", "Guest", null, Request.Scheme);
            return Challenge(new AuthenticationProperties { RedirectUri = redirectUrl }, GoogleDefaults.AuthenticationScheme);
        }

        // Callback sau khi Google xác thực thành công
        [HttpGet("GoogleResponse")]
        public async Task<IActionResult> GoogleResponse()
        {
            var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (!authenticateResult.Succeeded)
                return Unauthorized();

            var email = authenticateResult.Principal.FindFirst(ClaimTypes.Email)?.Value;
            var name = authenticateResult.Principal.FindFirst(ClaimTypes.Name)?.Value;

            if (email == null)
                return BadRequest("Failed to retrieve email from Google");

            var existingGuest = await _guestRepository.GetGuestByEmailAsync(email);
            if (existingGuest == null)
            {
                var newGuest = new Guest
                {
                    Email = email,
                    UserName = name,
                    Password = null,
                    Status = true,
                    CreateUser = DateTime.UtcNow
                };
                await _guestRepository.AddGuestAsync(newGuest);
            }

            return Ok(new { message = "Google login successful", email, name });
        }

        
    }
}
