using Auth.Core.Modules.Shared.Database;
using Auth.Core.Modules.User.Entities;
using Auth.Core.Modules.User.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Auth.Tests.Unit.User;

[TestFixture]
public class UserRepositoryTests
{
    [SetUp]
    public void Setup()
    {
        _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;

        _dbContext = new AppDbContext(_dbContextOptions);
        _repository = new UserRepository(_dbContext);
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    private DbContextOptions<AppDbContext> _dbContextOptions;
    private AppDbContext _dbContext;
    private UserRepository _repository;

    [Test]
    public async Task CreateUserAsync_Should_Add_User_To_Database()
    {
        // Arrange
        var user = new UserEntity
        {
            Email = "test@example.com",
            PasswordHash = "hashedpassword"
        };

        // Act
        await _repository.CreateUserAsync(user);

        // Assert
        var savedUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
        savedUser.Should().NotBeNull();
        savedUser!.Email.Should().Be(user.Email);
    }

    [Test]
    public async Task GetUserByEmailAsync_Should_Return_User_When_Email_Exists()
    {
        // Arrange
        var user = new UserEntity
        {
            Email = "test@example.com",
            PasswordHash = "hashedpassword"
        };
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _repository.GetUserByEmailAsync(user.Email);

        // Assert
        result.Should().NotBeNull();
        result!.Email.Should().Be(user.Email);
    }

    [Test]
    public async Task GetUserByEmailAsync_Should_Return_Null_When_Email_Does_Not_Exist()
    {
        // Act
        var result = await _repository.GetUserByEmailAsync("nonexistent@example.com");

        // Assert
        result.Should().BeNull();
    }
}