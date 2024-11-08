using BOs.DTO;
using BOs.Entity;
using DAOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REPOs
{
    public class AccountRepository : IAccountRepository
    {
        private readonly BookingRoommContext _context;
        private readonly IAccountDAO _accountDAO;

        public AccountRepository(BookingRoommContext context, IAccountDAO accountDAO)
        {
            _context = context;
            _accountDAO = accountDAO;
        }

        public Guest GetAccount(string email)
        {
            return _context.Guests.FirstOrDefault(g => g.Email == email);
        }

        public async Task<ChangePasswordResultDTO> ChangePasswordAsync(ChangePasswordDTO changePasswordDTO)
        {
            // Lấy thông tin người dùng từ email
            var guest = await _accountDAO.GetGuestByEmailAsync(changePasswordDTO.Email);

            // Kiểm tra nếu tài khoản không tồn tại
            if (guest == null)
            {
                return new ChangePasswordResultDTO
                {
                    Success = false,
                    Message = "Tài khoản không tồn tại."
                };
            }

            // So sánh mật khẩu hiện tại
            if (!BCrypt.Net.BCrypt.Verify(changePasswordDTO.CurrentPassword, guest.Password))
            {
                return new ChangePasswordResultDTO
                {
                    Success = false,
                    Message = "Mật khẩu hiện tại không chính xác."
                };
            }

            // Kiểm tra mật khẩu mới
            if (changePasswordDTO.NewPassword != changePasswordDTO.ConfirmNewPassword)
            {
                return new ChangePasswordResultDTO
                {
                    Success = false,
                    Message = "Mật khẩu mới không khớp."
                };
            }

            // Cập nhật mật khẩu mới (mã hóa lại)
            guest.Password = BCrypt.Net.BCrypt.HashPassword(changePasswordDTO.NewPassword);
            await _accountDAO.UpdateGuestAsync(guest);

            return new ChangePasswordResultDTO
            {
                Success = true,
                Message = "Mật khẩu đã được thay đổi thành công."
            };
        }

    }
}
