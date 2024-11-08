using BOs.DTO;
using BOs.Entity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REPOs
{
    public interface IAccountRepository
    {
        Guest GetAccount(string accountName);
        Task<ChangePasswordResultDTO> ChangePasswordAsync(ChangePasswordDTO changePasswordDTO);
    }
}
