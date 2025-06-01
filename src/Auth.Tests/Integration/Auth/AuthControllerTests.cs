using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Auth.Core.Modules.Auth;
using Auth.Core.Modules.Auth.DTOs;
using Auth.Core.Modules.Shared.Database;
using Auth.Core.Modules.Shared.DependencyInjection;
using Auth.Core.Modules.User.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Tests.Integration.Auth;

//TODO: This whole test needs to be refactored into many tests and a base class 
// For now it works.
[TestFixture]
public class AuthControllerTests
{
    [SetUp]
    public void Setup()
    {
        var services = new ServiceCollection();
        services.AddDbContext<AppDbContext>(options =>
            options.UseInMemoryDatabase($"TestDb_{Guid.NewGuid()}"));

        services.AddUserServices(); // Add your custom services
        services.AddAuthModule(); // Add authentication module

        var serviceProvider = services.BuildServiceProvider();

        // Seed the database if necessary
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.Users.Add(new UserEntity
        {
            Email = "joe@example.com",
            PasswordHash = "hashedpassword"
        });
        dbContext.SaveChanges();

        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services => { services.AddSingleton(serviceProvider); });
            });

        _client = _factory.CreateClient();
    }

    [TearDown]
    public async Task TearDown()
    {
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // Remove all entities from the database
        dbContext.Users.RemoveRange(dbContext.Users);
        await dbContext.SaveChangesAsync();

        // Dispose resources
        _client.Dispose();
        _factory.Dispose();
    }

    private WebApplicationFactory<Program> _factory = null!;
    private HttpClient _client = null!;

    [Test]
    public async Task Register_Returns_Success_For_Valid_Dto()
    {
        // Arrange
        var dto = new RegisterDto
        {
            Email = "joe3@example.com",
            Password = "password123"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/register", dto);

        // Debugging: Log response content if the test fails
        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response Status Code: {response.StatusCode}");
            Console.WriteLine($"Response Content: {content}");
        }

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        var jsonElement = await response.Content.ReadFromJsonAsync<JsonElement>();
        var message = jsonElement.GetProperty("message").GetString();
        message.Should().Be("User registered successfully");
    }

    [Test]
    public async Task Register_Returns_BadRequest_For_Invalid_Dto()
    {
        // Arrange
        var dto = new RegisterDto
        {
            Email = "",
            Password = "123" // Invalid password
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/register", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var jsonElement = await response.Content.ReadFromJsonAsync<JsonElement>();
        var errors = jsonElement.GetProperty("errors").EnumerateArray(); // Handle as an array

        // Validate that errors contain at least one item
        errors.Should().NotBeEmpty();
    }

    [Test]
    public async Task Login_Returns_Token_For_Valid_Credentials()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<UserEntity>>();

        var user = new UserEntity
        {
            Email = "joe@example.com",
        };
        user.PasswordHash = passwordHasher.HashPassword(user, "password123");

        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();

        var dto = new LoginDto
        {
            Email = "joe@example.com",
            Password = "password123"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/login", dto);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        var jsonElement = await response.Content.ReadFromJsonAsync<JsonElement>();
        var token = jsonElement.GetProperty("token").GetString();
        token.Should().NotBeNullOrEmpty();
    }

    [Test]
    public async Task Login_Returns_Unauthorized_For_Invalid_Credentials()
    {
        // Arrange
        var dto = new LoginDto
        {
            Email = "invalid@example.com",
            Password = "wrongpassword"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/login", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        var jsonElement = await response.Content.ReadFromJsonAsync<JsonElement>();
        var message = jsonElement.GetProperty("message").GetString();
        message.Should().Be("Invalid credentials");
    }
}