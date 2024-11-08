using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REPOs
{
    public interface IJWTService
    {
        string Generate(int accountId, string roleName);
    }
}
