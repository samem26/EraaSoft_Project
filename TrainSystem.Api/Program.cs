using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TrainSystem.Api.Data;
using TrainSystem.Api.Models;
using TrainSystem.Api.Repositories;

var builder = WebApplication.CreateBuilder(args);

string? configuredConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrWhiteSpace(configuredConnectionString))
{
    throw new InvalidOperationException(
        "Set ConnectionStrings:DefaultConnection to a SQL Server connection string. Use .NET User Secrets for local development.");
}

SqlConnectionStringBuilder sqlConnection = new SqlConnectionStringBuilder(configuredConnectionString);

if (string.IsNullOrWhiteSpace(sqlConnection.DataSource))
{
    throw new InvalidOperationException(
        "ConnectionStrings:DefaultConnection must specify a SQL Server host.");
}

if (string.IsNullOrWhiteSpace(sqlConnection.InitialCatalog))
{
    sqlConnection.InitialCatalog = "TrainSystemDb";
}

string connectionString = sqlConnection.ConnectionString;

builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
    options.User.RequireUniqueEmail = true;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using (IServiceScope scope = app.Services.CreateScope())
    {
        ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await dbContext.Database.MigrateAsync();
    }
}

app.UseAuthorization();

app.MapControllers();

app.Run();
