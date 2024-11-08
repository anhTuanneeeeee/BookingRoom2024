using BOs.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REPOs
{
    public interface IStatusRepository
    {
        Task<Status> CreateStatusAsync(int id, string name);
        Task<IEnumerable<Status>> GetAllStatusAsync();
        Task<Status?> GetStatusByIdAsync(int statusId);

    }
}
