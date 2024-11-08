using BOs.Entity;
using DAOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REPOs
{
    public class StatusRepository : IStatusRepository
    {
        private readonly IStatusDAO _statusDAO;

        public StatusRepository(IStatusDAO statusDAO)
        {
            _statusDAO = statusDAO;
        }

        public async Task<Status> CreateStatusAsync(int id, string name)
        {
            var status = new Status
            {
                StatusId = id,
                StatusName = name
            };
            return await _statusDAO.CreateStatusAsync(status);
        }
        public async Task<IEnumerable<Status>> GetAllStatusAsync()
        {
            return await _statusDAO.GetAllStatusAsync();
        }

        public async Task<Status?> GetStatusByIdAsync(int statusId)
        {
            return await _statusDAO.GetStatusByIdAsync(statusId);
        }

    }
}
