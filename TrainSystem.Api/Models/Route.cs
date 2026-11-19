namespace TrainSystem.Api.Models;

public class Route
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public ICollection<RouteStation> RouteStations { get; set; } = new List<RouteStation>();
    public ICollection<Journey> Journeys { get; set; } = new List<Journey>();
}
