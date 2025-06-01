using Auth.Core.Modules.Auth;
using Auth.Core.Modules.Shared.Database;
using Auth.Core.Modules.Shared.Database.Seed;
using Auth.Core.Modules.Shared.DependencyInjection;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options => { options.SuppressModelStateInvalidFilter = true; });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Custom modules
builder.Services.AddUserServices();
builder.Services.AddAuthModule();

// EF Core
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


// Required app pipeline setup
var app = builder.Build();

app.UseRouting();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("DatabaseSeeder");
    await DatabaseSeeder.SeedAsync(db, logger);
}

// Swagger for dev only
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Sample test routes
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast(
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();

        return forecast;
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();

app.MapGet("/dbtest", async (AppDbContext db) =>
{
    var user = await db.Users.FirstOrDefaultAsync();
    if (user == null)
        return Results.NotFound(new { status = "no users found" });

    return Results.Ok(new
    {
        status = "success",
        user = new
        {
            user.Id,
            user.Email
        }
    });
});

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}