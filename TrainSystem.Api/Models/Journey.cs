using TrainSystem.Api.Models.Enums;

namespace TrainSystem.Api.Models;

public class Journey
{
    public int Id { get; set; }

    // Calendar date on which the journey starts (Egypt local date).
    public DateOnly JourneyDate { get; set; }
    public JourneyStatus Status { get; set; }

    public int TrainId { get; set; }
    public Train Train { get; set; } = null!;

    public int RouteId { get; set; }
    public Route Route { get; set; } = null!;

    public ICollection<JourneyStop> JourneyStops { get; set; } = new List<JourneyStop>();
    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
