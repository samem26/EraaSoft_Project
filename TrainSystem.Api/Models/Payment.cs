using TrainSystem.Api.Models.Enums;

namespace TrainSystem.Api.Models;

public class Payment
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public PaymentStatus Status { get; set; }

    // Reference returned by the payment gateway. No card data is ever stored.
    public string? GatewayReference { get; set; }
    public DateTime? PaidAt { get; set; }

    public int BookingId { get; set; }
    public Booking Booking { get; set; } = null!;
}
