using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Auth.Core.Modules.Auth.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Auth.Tests.Integration;

[TestFixture]
public class AuthControllerTests
{
    [SetUp]
    public void Setup()
    {
        _factory = new WebApplicationFactory<Program>();
        _client = _factory.CreateClient();
    }

    [TearDown]
    public void TearDown()
    {
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
            Username = "John Doe",
            Password = "password123"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/register", dto);

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
            Username = "", // Invalid username
            Password = "123" // Invalid password
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/register", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var jsonElement = await response.Content.ReadFromJsonAsync<JsonElement>();
        var errors = jsonElement.GetProperty("errors");

        // Validate that errors contain at least one property
        errors.EnumerateObject().Should().NotBeEmpty();
    }
}