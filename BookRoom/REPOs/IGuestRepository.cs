using BOs.DTO;
using BOs.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REPOs
{
    public interface IGuestRepository
    {
       
        
            Task<Guest?> CreateCustomer(string userName, string email, string password, string phoneNumber);

            Task<Guest> Authenticate(string username, string password);

        Task<IEnumerable<Guest>> GetAllCustomersAsync();
        Task<Guest> GetCustomerByIdAsync(int id);

        Task<Guest> CreateStaff(string userName, string email, string password, string phoneNumber);
        Task<Guest?> GetGuestById(int id);
        Task<Guest> UpdateGuest(Guest guest);
        Task<bool> DeleteGuest(int id);
        Task<Guest?> GetGuestByEmailAsync(string email);
        Task AddGuestAsync(Guest guest);
        Task UpdateGuestAsync(Guest guest);

        Task<IEnumerable<StaffDTO>> GetAllStaffAsync();
        Task<StaffDTO?> GetStaffByIdAsync(int id);




    }
}
