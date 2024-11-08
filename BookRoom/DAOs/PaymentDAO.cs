using BOs.DTO;
using BOs.Entity;
using Microsoft.EntityFrameworkCore;
using Net.payOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAOs
{
    public class PaymentDAO : IPaymentDAO
    {
        private readonly BookingRoommContext _context;
        private readonly PayOS _payOS;

        public PaymentDAO(BookingRoommContext context, PayOS payOS)
        {
            _context = context;
            _payOS = payOS;
        }

        public async Task<Payment> CreatePaymentAsync(Payment payment)
        {
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
            return payment;
        }
        public async Task<IEnumerable<PaymentDTO>> GetAllPaymentsAsync()
        {
            var payments = await _context.Payments.ToListAsync();
            return payments.Select(p => new PaymentDTO
            {
                PaymentId = p.PaymentId,
                BookingId = p.BookingId,
                Id = p.Id,
                Amount = p.Amount,
                PaymentDate = p.PaymentDate,
                PaymentMethod = p.PaymentMethod,
                StatusPayment = p.StatusPayment
            });
        }

        public async Task<PaymentDTO?> GetPaymentByPaymentIdAsync(int paymentId)
        {
            var payment = await _context.Payments.FindAsync(paymentId);
            return payment == null ? null : new PaymentDTO
            {
                PaymentId = payment.PaymentId,
                BookingId = payment.BookingId,
                Id = payment.Id,
                Amount = payment.Amount,
                PaymentDate = payment.PaymentDate,
                PaymentMethod = payment.PaymentMethod,
                StatusPayment = payment.StatusPayment
            };
        }

        public async Task<IEnumerable<PaymentDTO>> GetPaymentByIdAsync(int id)
        {
            var payments = await _context.Payments
                                         .Where(p => p.Id == id)
                                         .ToListAsync();
            return payments.Select(p => new PaymentDTO
            {
                PaymentId = p.PaymentId,
                BookingId = p.BookingId,
                Id = p.Id,
                Amount = p.Amount,
                PaymentDate = p.PaymentDate,
                PaymentMethod = p.PaymentMethod,
                StatusPayment = p.StatusPayment
            });
        }

        public async Task<IEnumerable<PaymentDTO>> GetPaymentByStatusPaymentAsync(string statusPayment)
        {
            var payments = await _context.Payments
                                         .Where(p => p.StatusPayment == statusPayment)
                                         .ToListAsync();
            return payments.Select(p => new PaymentDTO
            {
                PaymentId = p.PaymentId,
                BookingId = p.BookingId,
                Id = p.Id,
                Amount = p.Amount,
                PaymentDate = p.PaymentDate,
                PaymentMethod = p.PaymentMethod,
                StatusPayment = p.StatusPayment
            });
        }

       
    }
}
