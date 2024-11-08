using BOs.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using REPOs;

namespace BookingController.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IGuestRepository _guestRepository;

        public CustomerController(IGuestRepository guestRepository)
        {
            _guestRepository = guestRepository;
        }

        [HttpPost("Register_CUSTOMER")]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerCreateDTO customerDto)
        {
            if (string.IsNullOrEmpty(customerDto.UserName) ||
                string.IsNullOrEmpty(customerDto.Email) ||
                string.IsNullOrEmpty(customerDto.Password) ||
                string.IsNullOrEmpty(customerDto.PhoneNumber))
            {
                return BadRequest("All fields are required.");
            }

            // Hash mật khẩu trước khi lưu
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(customerDto.Password);

            try
            {
                var customer = await _guestRepository.CreateCustomer(
                    customerDto.UserName, customerDto.Email, hashedPassword, customerDto.PhoneNumber
                );

                if (customer == null)
                {
                    return Conflict("Could not create customer.");
                }

                return Ok(customer);
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
                return StatusCode(500, "An error occurred while creating the customer.");
            }
        }
        [HttpGet("GetALL_CUSTOMER")]
        public async Task<IActionResult> GetAllCustomers()
        {
            var customers = await _guestRepository.GetAllCustomersAsync();
            return Ok(customers);
        }

        [HttpGet("GET_CUSTOMER_BY_ID")]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            var customer = await _guestRepository.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                return NotFound(new { message = "Customer not found" });
            }
            return Ok(customer);
        }

        [HttpPut("Update_Infor_Customer")]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] CustomerUpdateDTO customerDto)
        {
            // Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrEmpty(customerDto.UserName) ||
                string.IsNullOrEmpty(customerDto.Email) ||
                string.IsNullOrEmpty(customerDto.PhoneNumber))
            {
                return BadRequest("UserName, Email và PhoneNumber là bắt buộc.");
            }

            // Lấy thông tin khách hàng từ cơ sở dữ liệu
            var existingCustomer = await _guestRepository.GetGuestById(id);
            if (existingCustomer == null)
            {
                return NotFound("Không tìm thấy khách hàng.");
            }

            // Cập nhật thông tin khách hàng
            existingCustomer.UserName = customerDto.UserName;
            existingCustomer.Email = customerDto.Email;
            existingCustomer.PhoneNumber = customerDto.PhoneNumber;
            
            // Lưu thông tin cập nhật
            var updatedCustomer = await _guestRepository.UpdateGuest(existingCustomer);

            return Ok(updatedCustomer);
        }

        [HttpDelete("Delete_Customer_By_ID")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var existingCustomer = await _guestRepository.GetGuestById(id);
            if (existingCustomer == null)
            {
                return NotFound("Không tìm thấy tài khoản.");
            }

            var result = await _guestRepository.DeleteGuest(id);
            if (!result)
            {
                return StatusCode(500, "Có lỗi xảy ra khi xóa tài khoản.");
            }

            return Ok("Tài khoản đã được xóa thành công.");
        }
    }
}
