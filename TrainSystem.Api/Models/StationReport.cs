using TrainSystem.Api.Models.Enums;

namespace TrainSystem.Api.Models;

public class StationReport
{
    public int Id { get; set; }
    public StationEventType EventType { get; set; }

    // When the train actually arrived, departed or passed.
    public DateTime OccurredAt { get; set; }

    // When the officer submitted the report.
    public DateTime ReportedAt { get; set; }

    public int JourneyStopId { get; set; }
    public JourneyStop JourneyStop { get; set; } = null!;

    public string OfficerId { get; set; } = string.Empty;
    public ApplicationUser Officer { get; set; } = null!;
}
