namespace BookingController.Service
{
    public interface IPaymentService
    {
        Task<string> TuanTestPaymentAsync(long orderId);
    }
}
