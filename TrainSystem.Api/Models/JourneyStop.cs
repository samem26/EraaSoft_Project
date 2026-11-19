namespace TrainSystem.Api.Models;

// All DateTime values in the model are stored in UTC.
public class JourneyStop
{
    public int Id { get; set; }
    public int StopOrder { get; set; }

    // The first stop has no arrival and the last stop has no departure, so all times are nullable.
    public DateTime? ScheduledArrival { get; set; }
    public DateTime? ExpectedArrival { get; set; }
    public DateTime? ActualArrival { get; set; }
    public DateTime? ScheduledDeparture { get; set; }
    public DateTime? ExpectedDeparture { get; set; }
    public DateTime? ActualDeparture { get; set; }

    public int JourneyId { get; set; }
    public Journey Journey { get; set; } = null!;

    public int StationId { get; set; }
    public Station Station { get; set; } = null!;

    public ICollection<StationReport> StationReports { get; set; } = new List<StationReport>();
}
