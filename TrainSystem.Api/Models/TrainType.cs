namespace TrainSystem.Api.Models;

public class TrainType
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public ICollection<Train> Trains { get; set; } = new List<Train>();
    public ICollection<Fare> Fares { get; set; } = new List<Fare>();
}
