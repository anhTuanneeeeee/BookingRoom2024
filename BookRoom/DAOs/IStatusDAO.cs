using BOs.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAOs
{
    public interface IStatusDAO
    {
        Task<Status> CreateStatusAsync(Status status);
        Task<IEnumerable<Status>> GetAllStatusAsync();
        Task<Status?> GetStatusByIdAsync(int statusId);

    }
}
