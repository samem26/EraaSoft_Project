using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TrainSystem.Api.Models;
using Route = TrainSystem.Api.Models.Route;

namespace TrainSystem.Api.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<TrainType> TrainTypes => Set<TrainType>();
    public DbSet<Train> Trains => Set<Train>();
    public DbSet<Station> Stations => Set<Station>();
    public DbSet<Route> Routes => Set<Route>();
    public DbSet<RouteStation> RouteStations => Set<RouteStation>();
    public DbSet<Journey> Journeys => Set<Journey>();
    public DbSet<JourneyStop> JourneyStops => Set<JourneyStop>();
    public DbSet<Fare> Fares => Set<Fare>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<Ticket> Tickets => Set<Ticket>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<StationReport> StationReports => Set<StationReport>();
    public DbSet<Notification> Notifications => Set<Notification>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<IdentityRole>().HasData(
            new IdentityRole
            {
                Id = "a1b2c3d4-0001-4000-8000-000000000001",
                Name = "Admin",
                NormalizedName = "ADMIN",
                ConcurrencyStamp = "a1b2c3d4-0001-4000-8000-0000000000a1"
            },
            new IdentityRole
            {
                Id = "a1b2c3d4-0002-4000-8000-000000000002",
                Name = "StationOfficer",
                NormalizedName = "STATIONOFFICER",
                ConcurrencyStamp = "a1b2c3d4-0002-4000-8000-0000000000a2"
            },
            new IdentityRole
            {
                Id = "a1b2c3d4-0003-4000-8000-000000000003",
                Name = "Passenger",
                NormalizedName = "PASSENGER",
                ConcurrencyStamp = "a1b2c3d4-0003-4000-8000-0000000000a3"
            });

        builder.Entity<ApplicationUser>(entity =>
        {
            entity.Property(u => u.FullName).IsRequired().HasMaxLength(100);

            // An officer is assigned to one station. If the station is deleted the assignment is cleared.
            entity.HasOne(u => u.Station)
                .WithMany(s => s.Officers)
                .HasForeignKey(u => u.StationId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        builder.Entity<TrainType>(entity =>
        {
            entity.Property(t => t.Name).IsRequired().HasMaxLength(50);
            entity.Property(t => t.Description).HasMaxLength(250);
            entity.HasIndex(t => t.Name).IsUnique();
        });

        builder.Entity<Train>(entity =>
        {
            entity.Property(t => t.TrainNumber).IsRequired().HasMaxLength(20);
            entity.HasIndex(t => t.TrainNumber).IsUnique();
            entity.ToTable(t => t.HasCheckConstraint("CK_Trains_Capacity", "[Capacity] > 0"));

            entity.HasOne(t => t.TrainType)
                .WithMany(tt => tt.Trains)
                .HasForeignKey(t => t.TrainTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<Station>(entity =>
        {
            entity.Property(s => s.Name).IsRequired().HasMaxLength(100);
            entity.Property(s => s.City).IsRequired().HasMaxLength(100);
        });

        builder.Entity<Route>(entity =>
        {
            entity.Property(r => r.Name).IsRequired().HasMaxLength(100);
            entity.HasIndex(r => r.Name).IsUnique();
        });

        builder.Entity<RouteStation>(entity =>
        {
            entity.HasIndex(rs => new { rs.RouteId, rs.StopOrder }).IsUnique();
            entity.HasIndex(rs => new { rs.RouteId, rs.StationId }).IsUnique();
            entity.ToTable(t => t.HasCheckConstraint("CK_RouteStations_StopOrder", "[StopOrder] >= 1"));

            entity.HasOne(rs => rs.Route)
                .WithMany(r => r.RouteStations)
                .HasForeignKey(rs => rs.RouteId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(rs => rs.Station)
                .WithMany(s => s.RouteStations)
                .HasForeignKey(rs => rs.StationId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<Journey>(entity =>
        {
            entity.Property(j => j.Status).HasConversion<string>().HasMaxLength(20);

            entity.HasOne(j => j.Train)
                .WithMany(t => t.Journeys)
                .HasForeignKey(j => j.TrainId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(j => j.Route)
                .WithMany(r => r.Journeys)
                .HasForeignKey(j => j.RouteId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<JourneyStop>(entity =>
        {
            entity.HasIndex(js => new { js.JourneyId, js.StopOrder }).IsUnique();
            entity.HasIndex(js => new { js.JourneyId, js.StationId }).IsUnique();
            entity.ToTable(t => t.HasCheckConstraint("CK_JourneyStops_StopOrder", "[StopOrder] >= 1"));

            entity.HasOne(js => js.Journey)
                .WithMany(j => j.JourneyStops)
                .HasForeignKey(js => js.JourneyId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(js => js.Station)
                .WithMany(s => s.JourneyStops)
                .HasForeignKey(js => js.StationId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<Fare>(entity =>
        {
            entity.Property(f => f.TravelClass).HasConversion<string>().HasMaxLength(20);
            entity.Property(f => f.Price).HasPrecision(10, 2);
            entity.HasIndex(f => new { f.TrainTypeId, f.OriginStationId, f.DestinationStationId, f.TravelClass })
                .IsUnique();
            entity.ToTable(t =>
            {
                t.HasCheckConstraint("CK_Fares_Price", "[Price] >= 0");
                t.HasCheckConstraint("CK_Fares_Stations", "[OriginStationId] <> [DestinationStationId]");
            });

            entity.HasOne(f => f.TrainType)
                .WithMany(tt => tt.Fares)
                .HasForeignKey(f => f.TrainTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(f => f.OriginStation)
                .WithMany()
                .HasForeignKey(f => f.OriginStationId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(f => f.DestinationStation)
                .WithMany()
                .HasForeignKey(f => f.DestinationStationId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<Booking>(entity =>
        {
            entity.Property(b => b.TotalAmount).HasPrecision(10, 2);
            entity.Property(b => b.Status).HasConversion<string>().HasMaxLength(20);

            entity.HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<Ticket>(entity =>
        {
            entity.Property(t => t.TravelClass).HasConversion<string>().HasMaxLength(20);
            entity.Property(t => t.PricePaid).HasPrecision(10, 2);

            // Whole-journey seat policy: one seat number can be used once per journey.
            entity.HasIndex(t => new { t.JourneyId, t.SeatNumber }).IsUnique();
            entity.ToTable(t =>
            {
                t.HasCheckConstraint("CK_Tickets_SeatNumber", "[SeatNumber] >= 1");
                t.HasCheckConstraint("CK_Tickets_Stations", "[OriginStationId] <> [DestinationStationId]");
            });

            entity.HasOne(t => t.Booking)
                .WithMany(b => b.Tickets)
                .HasForeignKey(t => t.BookingId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(t => t.Journey)
                .WithMany(j => j.Tickets)
                .HasForeignKey(t => t.JourneyId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(t => t.OriginStation)
                .WithMany()
                .HasForeignKey(t => t.OriginStationId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(t => t.DestinationStation)
                .WithMany()
                .HasForeignKey(t => t.DestinationStationId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<Payment>(entity =>
        {
            entity.Property(p => p.Amount).HasPrecision(10, 2);
            entity.Property(p => p.Status).HasConversion<string>().HasMaxLength(20);
            entity.Property(p => p.GatewayReference).HasMaxLength(100);

            // SQL Server only enforces this on rows where GatewayReference is not null.
            entity.HasIndex(p => p.GatewayReference).IsUnique();

            entity.HasOne(p => p.Booking)
                .WithMany(b => b.Payments)
                .HasForeignKey(p => p.BookingId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<StationReport>(entity =>
        {
            entity.Property(r => r.EventType).HasConversion<string>().HasMaxLength(20);

            entity.HasOne(r => r.JourneyStop)
                .WithMany(js => js.StationReports)
                .HasForeignKey(r => r.JourneyStopId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(r => r.Officer)
                .WithMany(u => u.StationReports)
                .HasForeignKey(r => r.OfficerId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<Notification>(entity =>
        {
            entity.Property(n => n.Message).IsRequired().HasMaxLength(500);

            entity.HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(n => n.Journey)
                .WithMany(j => j.Notifications)
                .HasForeignKey(n => n.JourneyId)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
