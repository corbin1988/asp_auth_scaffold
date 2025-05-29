using Auth.Core.Modules.User.Entities;
using Microsoft.EntityFrameworkCore;

namespace Auth.Core.Modules.Shared.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<UserEntity> Users { get; set; }

    //  Loads fluent config (if you use IEntityTypeConfiguration<T>)
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        // Optional: Inline config if not using separate config classes
        // modelBuilder.Entity<UserEntity>().HasKey(x => x.Id);
    }
}