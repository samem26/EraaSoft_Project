# Trains

**Train Ticket Booking and Estimated Tracking System**

Trains is a graduation project developed as part of the EraaSoft diploma using ASP.NET Core Web API.

The system aims to make train ticket booking easier by allowing passengers to search for journeys, book tickets between stations, and follow estimated train progress.

Station officers report train arrivals and departures, allowing the system to update expected arrival times when delays occur.

## Technologies

- C# / .NET 10
- ASP.NET Core Web API
- Entity Framework Core (Code First)
- Microsoft SQL Server
- ASP.NET Core Identity
- Repository Pattern

## Main Features

The system is designed to support:

- **User Accounts:** Registration, login, and role-based access.
- **Train Management:** Manage trains, train types, stations, and routes.
- **Journey Scheduling:** Create journeys with departure times and intermediate stops.
- **Journey Search:** Find trains travelling between selected stations.
- **Ticket Booking:** Reserve seats between boarding and destination stations.
- **Seat Availability:** Allow seats to be booked again on non-overlapping parts of the same journey.
- **Payments:** Pay for ticket bookings.
- **Station Reports:** Station officers report train arrivals, departures, and passage events.
- **Estimated Tracking:** Display estimated train progress based on schedules and station reports, without GPS.
- **Delay Notifications:** Inform passengers about changes to expected arrival times.

### User Roles

| Role            | Responsibilities                                                              |
| --------------- | ----------------------------------------------------------------------------- |
| Admin           | Manage trains, stations, routes, schedules, fares, and station officers.      |
| Station Officer | Report train arrivals, departures, and passage events at assigned stations.   |
| Passenger       | Search for journeys, book tickets, make payments, and follow journey updates. |

## Current Progress

The project currently includes:

- ASP.NET Core Web API setup
- Database entities and EF Core Code First
- ASP.NET Core Identity foundation
- Generic Repository Pattern
- Initial database migration
- Basic station retrieval endpoints

The remaining features are under development.

## Getting Started

### Requirements

- .NET 10 SDK
- Microsoft SQL Server
- Git

### 1. Clone the Repository

```bash
git clone https://github.com/samem26/EraaSoft_Project.git
cd EraaSoft_Project
```

The repository is private, so you must have access to it.

### 2. Run the Application

```bash
dotnet run --project TrainSystem.Api
```

The default local address is:

`http://localhost:5080`

## Available API Endpoints

| Method | Endpoint             | Description         |
| ------ | -------------------- | ------------------- |
| GET    | `/api/stations`      | Get all stations    |
| GET    | `/api/stations/{id}` | Get a station by ID |

Additional endpoints will be added as development progresses.

## Project Structure

```text
EraaSoft_Project/
├── TrainSystem.Api/
│   ├── Controllers/
│   ├── Data/
│   ├── Models/
│   ├── Repositories/
│   ├── Migrations/
│   └── Program.cs
├── docs/
├── EraaSoft_Project.slnx
└── README.md
```

## Documentation

Project requirements and ERD diagrams are available in the `docs/` directory.

---

**Project:** EraaSoft Diploma Graduation Project

**Status:** Under Development
