using Net.payOS;

namespace BookingController.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly PayOS _payOS;
        private readonly ILogger<PaymentService> _logger;

        public PaymentService(PayOS payOS, ILogger<PaymentService> logger)
        {
            _payOS = payOS;
            _logger = logger;
        }

        public async Task<string> TuanTestPaymentAsync(long orderId)
        {
            _logger.LogInformation($"Starting TuanTestPaymentAsync with orderId: {orderId}");

            try
            {
                // Gọi phương thức getPaymentLinkInformation của PayOS
                var paymentInfo = await _payOS.getPaymentLinkInformation(orderId);

                // Log kết quả từ API
                _logger.LogInformation($"PayOS response for orderId {orderId}: {paymentInfo}");

                // Trả về thông tin thanh toán dưới dạng chuỗi JSON
                return paymentInfo != null ? $"Payment Info: {paymentInfo}" : "Payment info not found";
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in TuanTestPaymentAsync: {ex.Message}");
                return $"Error: {ex.Message}";
            }
        }
    }
}
