using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BookingController.Controllers
{
    public class ApiResponse<T> 
    {
        public int Status { get; set; }
        public string StatusText { get; set; }
        public T Data { get; set; }
        public object Headers { get; set; }
    }
}