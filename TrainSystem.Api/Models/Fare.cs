using TrainSystem.Api.Models.Enums;

namespace TrainSystem.Api.Models;

public class Fare
{
    public int Id { get; set; }
    public TravelClass TravelClass { get; set; }
    public decimal Price { get; set; }

    public int TrainTypeId { get; set; }
    public TrainType TrainType { get; set; } = null!;

    public int OriginStationId { get; set; }
    public Station OriginStation { get; set; } = null!;

    public int DestinationStationId { get; set; }
    public Station DestinationStation { get; set; } = null!;
}
