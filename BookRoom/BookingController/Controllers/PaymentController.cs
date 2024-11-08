using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Net.payOS.Types;
using Net.payOS;
using BookingController.Types;
using REPOs;
using BOs.Entity;
using BOs.DTO;
using BookingController.Service;

namespace BookingController.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly PayOS _payOS;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IPaymentService _paymentService;

        public PaymentController(PayOS payOS, IPaymentRepository paymentRepository, IPaymentService paymentService)
        {
            _payOS = payOS;
            _paymentRepository = paymentRepository;
            _paymentService = paymentService;
        }

        [HttpPost("payos_transfer_handler")]
        public IActionResult PayOSTransferHandler(WebhookType body)
        {
            try
            {
                WebhookData data = _payOS.verifyPaymentWebhookData(body);

                if (data.description == "Transaction successful")
                {
                    return Ok(new Response(0, "Payment verified", null));
                }
                return Ok(new Response(0, "Payment received", null));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Ok(new Response(-1, "Verification failed", null));
            }
        }
        [HttpGet("GetAllPayments")]
        public async Task<ActionResult<IEnumerable<PaymentDTO>>> GetAllPayments()
        {
            var payments = await _paymentRepository.GetAllPaymentsAsync();
            return Ok(payments);
        }

        [HttpGet("GetPaymentByPaymentId/{paymentId}")]
        public async Task<ActionResult<PaymentDTO>> GetPaymentByPaymentId(int paymentId)
        {
            var payment = await _paymentRepository.GetPaymentByPaymentIdAsync(paymentId);
            if (payment == null)
                return NotFound();

            return Ok(payment);
        }

        [HttpGet("GetPaymentByCustomerId")]
        public async Task<ActionResult<IEnumerable<PaymentDTO>>> GetPaymentById(int id)
        {
            var payments = await _paymentRepository.GetPaymentByIdAsync(id);
            return Ok(payments);
        }

        [HttpGet("GetPaymentByStatusPayment/CANCELLEDorPENDINGorPAID")]
        public async Task<ActionResult<IEnumerable<PaymentDTO>>> GetPaymentByStatusPayment(string statusPayment)
        {
            var payments = await _paymentRepository.GetPaymentByStatusPaymentAsync(statusPayment);
            return Ok(payments);
        }
        [HttpGet("TuanTestPayment/{orderId}")]
        public async Task<IActionResult> TuanTestPayment(long orderId)
        {
            var result = await _paymentService.TuanTestPaymentAsync(orderId);

            // Trả về kết quả từ PaymentService
            return Ok(new { message = result });
        }




    }
}
