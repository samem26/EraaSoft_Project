namespace TrainSystem.Api.Models;

public class Station
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;

    public ICollection<ApplicationUser> Officers { get; set; } = new List<ApplicationUser>();
    public ICollection<RouteStation> RouteStations { get; set; } = new List<RouteStation>();
    public ICollection<JourneyStop> JourneyStops { get; set; } = new List<JourneyStop>();
}
