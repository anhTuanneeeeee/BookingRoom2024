using BOs.DTO;
using BOs.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REPOs
{
    public interface IPaymentRepository
    {
        Task<Payment> CreatePaymentAsync(Payment payment);
        Task UpdatePaymentStatus(int paymentId, string status);
        Task<Payment?> GetPaymentByOrderCode(long orderCode);
        Task<IEnumerable<PaymentDTO>> GetAllPaymentsAsync();
        Task<PaymentDTO?> GetPaymentByPaymentIdAsync(int paymentId);
        Task<IEnumerable<PaymentDTO>> GetPaymentByIdAsync(int id);
        Task<IEnumerable<PaymentDTO>> GetPaymentByStatusPaymentAsync(string statusPayment);


    }
}
