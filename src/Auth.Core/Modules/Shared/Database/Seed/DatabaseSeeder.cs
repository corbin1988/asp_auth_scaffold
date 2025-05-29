using Auth.Core.Modules.User.Entities;

namespace Auth.Core.Modules.Shared.Database.Seed;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(AppDbContext db, ILogger logger)
    {
        if (!db.Users.Any())
        {
            db.Users.Add(new UserEntity
            {
                Id = Guid.NewGuid(),
                Email = "admin@example.com",
                PasswordHash = "hashedpassword"
            });

            await db.SaveChangesAsync();
            logger.LogInformation("Seeded admin user");
        }
    }
}