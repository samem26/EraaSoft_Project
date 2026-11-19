namespace TrainSystem.Api.Models;

public class Train
{
    public int Id { get; set; }
    public string TrainNumber { get; set; } = string.Empty;
    public int Capacity { get; set; }

    public int TrainTypeId { get; set; }
    public TrainType TrainType { get; set; } = null!;

    public ICollection<Journey> Journeys { get; set; } = new List<Journey>();
}
