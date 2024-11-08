using BOs.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAOs
{
    public interface IGuestDAO
    {
        Task<Guest> CreateGuest(Guest guest);
        Task<Guest?> GetGuestByEmail(string email);
        Task<Guest?> GetGuestByPhoneNumber(string phoneNumber);
        Task<IEnumerable<Guest>> GetAllGuestsAsync();
        Task<Guest> GetGuestByIdAsync(int id);
        Task<Guest> CreateGuestAsync(Guest guest);
        Task<Guest> UpdateGuest(Guest guest);
        Task<Guest?> GetGuestById(int id);
        Task<bool> DeleteGuest(int id);

    }
}
