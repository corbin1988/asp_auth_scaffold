using Auth.Core.Modules.Shared.Database;
using Microsoft.EntityFrameworkCore;

namespace Auth.Tests.Unit;

[TestFixture]
public class AppDbContextTests : IDisposable
{
    private AppDbContext _context = null!;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("TestDb")
            .Options;

        _context = new AppDbContext(options);

        // Optionally seed data here
    }

    [Test]
    public void CanAddUser()
    {
        // Arrange
        var user = new Auth.Core.Modules.User.Entities.UserEntity
        {
            Email = "test@example.com",
            PasswordHash = "hash"
        };

        // Act
        _context.Users.Add(user);
        _context.SaveChanges();

        // Assert
        Assert.That(_context.Users.Any(u => u.Email == "test@example.com"), Is.True);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}