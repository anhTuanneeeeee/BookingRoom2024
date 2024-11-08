using BOs.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAOs
{
    public interface IAccountDAO
    {
        Task<Guest?> GetGuestByEmailAsync(string email);
        Task UpdateGuestAsync(Guest guest);
    }
}
