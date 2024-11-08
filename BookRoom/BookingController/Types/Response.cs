namespace BookingController.Types
{
    public record Response(
         int error,
         string message,
         object? data
     );
}
