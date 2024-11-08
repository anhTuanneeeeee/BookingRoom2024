using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOs.DTO
{
    public class PaymentDTO
    {
        public int PaymentId { get; set; }
        public int BookingId { get; set; }
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string? PaymentMethod { get; set; }
        public string? StatusPayment { get; set; }
    }
}
