# Trains

**Train Ticket Booking and Estimated Tracking System**

An EraaSoft graduation project built with ASP.NET Core Web API.

Trains aims to let passengers search for train journeys, book tickets between stations, and track estimated train progress. Station officers report train arrivals and departures to help update expected arrival times.

## Technologies

- C# / .NET 10
- ASP.NET Core Web API
- Entity Framework Core (Code First)
- SQL Server
- ASP.NET Core Identity
- Repository Pattern

## Features

The system is designed to support:

- Passenger registration and login
- Train, station, route, and schedule management
- Journey search and ticket booking
- Seat availability between stations
- Ticket payments
- Station officer reports
- Estimated train tracking and delay notifications
- Admin management

**Current progress:** The database models, Identity foundation, EF Core migrations, generic repository, and basic station endpoints are implemented. The remaining features are under development.

## Getting Started

**Requirements:** .NET 10 SDK, SQL Server, and Git.

**1. Clone the repository**

```bash
git clone https://github.com/samem26/EraaSoft_Project.git
cd EraaSoft_Project
```

**2. Configure your database**

Set your SQL Server connection string using .NET User Secrets:

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" \
  "Server=localhost,1433;Database=TrainSystemDb;User Id=sa;Password=YOUR_PASSWORD;TrustServerCertificate=True;" \
  --project TrainSystem.Api
```

Replace the server, port, username, and password with your own values. Keep credentials out of Git.

**3. Run the application**

```bash
dotnet run --project TrainSystem.Api
```

In Development, EF Core automatically creates the database if necessary and applies pending migrations. No manual database migration command is required.

## Available Endpoints

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/stations` | Get all stations |
| GET | `/api/stations/{id}` | Get a station by ID |

Default local URL: `http://localhost:5080`

## Project Structure

- `TrainSystem.Api/Controllers` — API endpoints
- `TrainSystem.Api/Models` — Database entities
- `TrainSystem.Api/Data` — EF Core DbContext
- `TrainSystem.Api/Repositories` — Generic repository
- `TrainSystem.Api/Migrations` — Code First migrations
- `docs/` — Project documentation and database diagrams

## Development Note

The planned booking system allows a seat to be reused after a passenger reaches their destination station. The current database constraint still reserves a seat for the entire journey; this will be corrected when implementing booking logic.