namespace TrainSystem.Api.Models;

public class RouteStation
{
    public int Id { get; set; }
    public int StopOrder { get; set; }

    public int RouteId { get; set; }
    public Route Route { get; set; } = null!;

    public int StationId { get; set; }
    public Station Station { get; set; } = null!;
}
