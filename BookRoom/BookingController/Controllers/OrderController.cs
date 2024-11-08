using BookingController.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Net.payOS.Types;
using Net.payOS;
using REPOs;
using Microsoft.EntityFrameworkCore;
using BOs.Entity;

namespace BookingController.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly PayOS _payOS;
        private readonly IBookingRepository _bookingRepository;
        private readonly ISlotRepository _slotRepository;
        private readonly BookingRoommContext _context;
        private readonly IPaymentRepository _paymentRepository;

        public OrderController(PayOS payOS, IBookingRepository bookingRepository, ISlotRepository slotRepository, IPaymentRepository paymentRepository, BookingRoommContext context)
        {
            _payOS = payOS;
            _bookingRepository = bookingRepository;
            _slotRepository = slotRepository;
            _paymentRepository = paymentRepository;
            _context = context;
        }
        [HttpPost("create-payment-link")]
        public async Task<IActionResult> CreatePaymentLink(int bookingId, string returnUrl, string cancelUrl)
        {
            try
            {
                var booking = await _bookingRepository.GetBookingByIdAsync(bookingId);
                if (booking == null)
                {
                    return NotFound(new { Message = "Booking not found" });
                }

                int orderCode = int.Parse(DateTimeOffset.Now.ToString("ffffff"));
                decimal amount = (decimal)booking.TotalFee; // Assuming TotalFee is already calculated

                ItemData item = new ItemData("Booking " + bookingId, 1, (int)amount);
                List<ItemData> items = new List<ItemData> { item };
                PaymentData paymentData = new PaymentData(orderCode, (int)amount, "Booking Payment", items, cancelUrl, returnUrl);

                CreatePaymentResult createPayment = await _payOS.createPaymentLink(paymentData);

                // Tạo bản ghi thanh toán trong cơ sở dữ liệu
                var payment = new Payment
                {
                    PaymentId = orderCode,
                    BookingId = bookingId,
                    Id = (int)booking.Id,
                    Amount = amount,
                    PaymentDate = DateTime.Now,
                    PaymentMethod = "PayOS",
                    StatusId = 2, // Assuming status 2 is "Slot Being Reserved"
                    StatusPayment = "PENDING"

                };
                await _paymentRepository.CreatePaymentAsync(payment);

                // Cập nhật trạng thái của Slot liên quan trong booking thành "Slot Being Reserved"
                foreach (var slotBooking in booking.SlotBookings)
                {
                    await _slotRepository.UpdateSlotStatusByIdAsync(slotBooking.SlotId, 2); // Assuming status 2 is "Slot Being Reserved"
                }

                return Ok(new { Message = "Payment link created successfully", PaymentId = orderCode, Link = createPayment.checkoutUrl });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, new { Message = "Failed to create payment link" });
            }
        }

        [HttpPost("createeee/{bookingId}")] // Thay đổi tham số nhận vào
        public async Task<IActionResult> CreatePaymentLink(int bookingId)
        {
            try
            {
                // Lấy thông tin booking từ ID
                var booking = await _bookingRepository.GetBookingByIdAsync(bookingId);
                if (booking == null)
                {
                    return NotFound(new Response(-1, "Booking not found", null));
                }

                // Tạo payment link
                int orderCode = int.Parse(DateTimeOffset.Now.ToString("ffffff"));
                ItemData item = new ItemData("Booking " + bookingId, 1, (int)booking.TotalFee); // Sử dụng giá từ booking
                List<ItemData> items = new List<ItemData> { item };
                PaymentData paymentData = new PaymentData(orderCode, (int)booking.TotalFee, "Payment for booking", items, "https://www.facebook.com/mt17th5/", "https://www.facebook.com/university.fpt.edu.vn");

                CreatePaymentResult createPayment = await _payOS.createPaymentLink(paymentData);

                return Ok(new Response(0, "Payment link created successfully", createPayment));
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return Ok(new Response(-1, "Failed to create payment link", null));
            }
        }

        /*[HttpPost("create")]
        public async Task<IActionResult> CreatePaymentLink(CreatePaymentLinkRequest body)
        {
            try
            {
                int orderCode = int.Parse(DateTimeOffset.Now.ToString("ffffff"));
                ItemData item = new ItemData(body.productName, 1, body.price);
                List<ItemData> items = new List<ItemData> { item };
                PaymentData paymentData = new PaymentData(orderCode, body.price, body.description, items, body.cancelUrl, body.returnUrl);

                CreatePaymentResult createPayment = await _payOS.createPaymentLink(paymentData);

                return Ok(new Response(0, "Payment link created successfully", createPayment));
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return Ok(new Response(-1, "Failed to create payment link", null));
            }
        }
*/

        [HttpGet("GetOrderID")]
        public async Task<IActionResult> GetOrder( long orderId)
        {
            try
            {
                PaymentLinkInformation paymentLinkInformation = await _payOS.getPaymentLinkInformation(orderId);
                return Ok(new Response(0, "Order information retrieved", paymentLinkInformation));
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return Ok(new Response(-1, "Failed to retrieve order information", null));
            }
        }
        
        [HttpGet("GetOderById/{orderCode}")]
        public async Task<IActionResult> GetOrderByOrderCode(long orderCode)
        {
            try
            {
                // Lấy thông tin thanh toán từ API PayOS dựa trên orderCode
                PaymentLinkInformation paymentLinkInformation = await _payOS.getPaymentLinkInformation(orderCode);

                // Lấy Payment từ database dựa trên orderCode
                var payment = await _paymentRepository.GetPaymentByOrderCode(orderCode);

                if (payment != null)
                {
                    if (paymentLinkInformation.status == "CANCELLED")
                    {
                        // Cập nhật statusPayment thành "CANCELLED" trong bảng Payment
                        await _paymentRepository.UpdatePaymentStatus(payment.PaymentId, "CANCELLED");
                        


                        payment.StatusId = 1;
                        await _context.SaveChangesAsync();

                        // Lấy BookingId từ Payment
                        await _slotRepository.UpdateSlotStatusByBookingId(payment.BookingId, 1);
                        var booking = await _context.Bookings
                                            .Include(b => b.Slot)
                                            .FirstOrDefaultAsync(b => b.BookingId == payment.BookingId);

                        if (booking?.Slot != null)
                        {
                            booking.Slot.StatusId = 1;
                            await _context.SaveChangesAsync();
                        }

                    }
                    else if (paymentLinkInformation.status == "PENDING")
                    {
                        // Đảm bảo status của các slot giữ nguyên là 2 (đang đặt phòng)
                        await _slotRepository.UpdateSlotStatusByBookingId(payment.BookingId, 2);
                    }
                    else if (paymentLinkInformation.status == "PAID")
                    {
                        await _paymentRepository.UpdatePaymentStatus(payment.PaymentId, "PAID");

                       

                        payment.StatusId = 3;
                        await _context.SaveChangesAsync();

                        await _slotRepository.UpdateSlotStatusByBookingId(payment.BookingId, 3);
                        var booking = await _context.Bookings
                                           .Include(b => b.Slot)
                                           .FirstOrDefaultAsync(b => b.BookingId == payment.BookingId);

                        if (booking?.Slot != null)
                        {
                            booking.Slot.StatusId = 3;
                            await _context.SaveChangesAsync();
                        }

                    }
                }

                return Ok(new Response(0, "Order information retrieved", paymentLinkInformation));
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return Ok(new Response(-1, "Failed to retrieve order information", null));
            }
        }

        [HttpPut("{orderId}")]
        public async Task<IActionResult> CancelOrder([FromRoute] int orderId)
        {
            try
            {
                PaymentLinkInformation paymentLinkInformation = await _payOS.cancelPaymentLink(orderId);
                return Ok(new Response(0, "Order cancelled successfully", paymentLinkInformation));
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return Ok(new Response(-1, "Failed to cancel order", null));
            }
        }
        /* [HttpPost("payment")]
         public async Task<IActionResult> CompletePayment(int bookingId)
         {

             bool isPaymentSuccessful = true; 

             if (isPaymentSuccessful)
             {
                 // Lấy thông tin booking
                 var booking = await _bookingRepository.GetBookingByIdAsync(bookingId);
                 if (booking == null)
                 {
                     return NotFound("Booking not found");
                 }

                 // Lấy slot từ booking
                 var slot = await _context.Slots.FindAsync(booking.SlotId);
                 if (slot == null)
                 {
                     return NotFound("Slot not found");
                 }

                 // Cập nhật trạng thái của slot thành "Slot Reserved"
                 slot.StatusId = 3; 
                 await _context.SaveChangesAsync();

                 return Ok("Payment completed successfully");
             }

             return BadRequest("Payment failed");
         }*/

        [HttpPost("confirm-webhook")]
        public async Task<IActionResult> ConfirmWebhook(ConfirmWebhook body)
        {
            try
            {
                await _payOS.confirmWebhook(body.webhook_url);
                return Ok(new Response(0, "Ok", null));
            }
            catch (System.Exception exception)
            {

                Console.WriteLine(exception);
                return Ok(new Response(-1, "fail", null));
            }

        }
    }
}
