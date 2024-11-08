using BOs.DTO;
using BOs.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REPOs
{
    public interface IBranchRepository
    {
        Task AddBranch(Branch branch);
        Task<Branch?> GetBranchById(int id);
        Task<List<Branch>> GetAllBranches();
        Task<List<Branch>> SearchBranches(string searchTerm);
        Task DeleteBranch(int id);
        Task UpdateBranch(Branch branch);
    }
}
