using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REPOs
{
    public class ApiResponse<T>
    {
        public int Status { get; set; }
        public string StatusText { get; set; }
        public T Data { get; set; }

        public ApiResponse(int status, string statusText, T data)
        {
            Status = status;
            StatusText = statusText;
            Data = data;
        }
    }
}
