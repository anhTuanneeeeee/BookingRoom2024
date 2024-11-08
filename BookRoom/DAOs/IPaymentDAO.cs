using BOs.DTO;
using BOs.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAOs
{
    public interface IPaymentDAO
    {
        Task<Payment> CreatePaymentAsync(Payment payment);
        Task<IEnumerable<PaymentDTO>> GetAllPaymentsAsync();
        Task<PaymentDTO?> GetPaymentByPaymentIdAsync(int paymentId);
        Task<IEnumerable<PaymentDTO>> GetPaymentByIdAsync(int id);
        Task<IEnumerable<PaymentDTO>> GetPaymentByStatusPaymentAsync(string statusPayment);


    }
}
