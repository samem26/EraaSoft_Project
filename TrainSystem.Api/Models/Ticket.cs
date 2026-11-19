using TrainSystem.Api.Models.Enums;

namespace TrainSystem.Api.Models;

public class Ticket
{
    public int Id { get; set; }
    public int SeatNumber { get; set; }
    public TravelClass TravelClass { get; set; }

    // Price copied from the fare when the ticket is booked.
    public decimal PricePaid { get; set; }

    public int BookingId { get; set; }
    public Booking Booking { get; set; } = null!;

    public int JourneyId { get; set; }
    public Journey Journey { get; set; } = null!;

    public int OriginStationId { get; set; }
    public Station OriginStation { get; set; } = null!;

    public int DestinationStationId { get; set; }
    public Station DestinationStation { get; set; } = null!;
}
