using Auth.Core.Modules.Shared.Database;
using Auth.Core.Modules.User.Entities;
using Microsoft.EntityFrameworkCore;

namespace Auth.Tests.Unit;

[TestFixture]
public class AppDbContextTests : IDisposable
{
    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("TestDb")
            .Options;

        _context = new AppDbContext(options);

        // Optionally seed data here
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    private AppDbContext _context = null!;

    [Test]
    public void CanAddUser()
    {
        // Arrange
        var user = new UserEntity
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

    public void Dispose()
    {
        _context.Dispose();
    }
}