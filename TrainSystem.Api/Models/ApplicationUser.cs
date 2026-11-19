using Microsoft.AspNetCore.Identity;

namespace TrainSystem.Api.Models;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;

    // Only set for users in the StationOfficer role.
    public int? StationId { get; set; }
    public Station? Station { get; set; }

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    public ICollection<StationReport> StationReports { get; set; } = new List<StationReport>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
