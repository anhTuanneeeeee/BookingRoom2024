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
    public class PaymentRepository : IPaymentRepository
    {
        private readonly IPaymentDAO _paymentDAO;
        private readonly BookingRoommContext _context;

        public PaymentRepository(IPaymentDAO paymentDAO, BookingRoommContext context)
        {
            _paymentDAO = paymentDAO;
            _context = context;
        }

        public async Task<Payment> CreatePaymentAsync(Payment payment)
        {
            return await _paymentDAO.CreatePaymentAsync(payment);
        }

        public async Task UpdatePaymentStatus(int paymentId, string status)
        {
            var payment = await _context.Payments.FirstOrDefaultAsync(p => p.PaymentId == paymentId);
            if (payment != null)
            {
                payment.StatusPayment = status;
                await _context.SaveChangesAsync();
            }
        }
        public async Task<Payment?> GetPaymentByOrderCode(long orderCode)
        {
            return await _context.Payments.FirstOrDefaultAsync(p => p.PaymentId == orderCode);
        }
        public async Task<IEnumerable<PaymentDTO>> GetAllPaymentsAsync()
        {
            // Thêm logic nghiệp vụ nếu cần thiết
            return await _paymentDAO.GetAllPaymentsAsync();
        }

        public async Task<PaymentDTO?> GetPaymentByPaymentIdAsync(int paymentId)
        {
            // Thêm logic nghiệp vụ nếu cần thiết
            return await _paymentDAO.GetPaymentByPaymentIdAsync(paymentId);
        }

        public async Task<IEnumerable<PaymentDTO>> GetPaymentByIdAsync(int id)
        {
            // Thêm logic nghiệp vụ nếu cần thiết
            return await _paymentDAO.GetPaymentByIdAsync(id);
        }

        public async Task<IEnumerable<PaymentDTO>> GetPaymentByStatusPaymentAsync(string statusPayment)
        {
            // Thêm logic nghiệp vụ nếu cần thiết
            return await _paymentDAO.GetPaymentByStatusPaymentAsync(statusPayment);
        }
    }
}
