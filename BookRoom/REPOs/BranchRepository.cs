using BOs.DTO;
using BOs.Entity;
using DAOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REPOs
{
    public class BranchRepository : IBranchRepository
    {
        private readonly IBranchDAO _branchDAO;

        public BranchRepository(IBranchDAO branchDAO)
        {
            _branchDAO = branchDAO;
        }

        public async Task AddBranch(Branch branch)
        {
            await _branchDAO.AddBranch(branch);
        }

        public async Task<Branch?> GetBranchById(int id)
        {
            return await _branchDAO.GetBranchById(id);
        }

        public async Task<List<Branch>> GetAllBranches()
        {
            return await _branchDAO.GetAllBranches();
        }

        public async Task<List<Branch>> SearchBranches(string searchTerm)
        {
            return await _branchDAO.SearchBranches(searchTerm);
        }

        public async Task DeleteBranch(int id)
        {
            await _branchDAO.DeleteBranch(id);
        }

        public async Task UpdateBranch(Branch branch)
        {
            await _branchDAO.UpdateBranch(branch);
        }
    }
}
