using System.IdentityModel.Tokens.Jwt;
using Auth.Core.Modules.Shared.JWT;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using NSubstitute;

namespace Auth.Tests.Unit.Shared;

[TestFixture]
public class TokenGeneratorTests
{
    private IConfiguration _configMock = null!;
    private TokenGenerator _tokenGenerator = null!;

    [SetUp]
    public void Setup()
    {
        _configMock = Substitute.For<IConfiguration>();
        _configMock["Jwt:Key"].Returns("YourSuperSecretKey1234567890123456");
        _configMock["Jwt:Issuer"].Returns("YourIssuer");
        _configMock["Jwt:Audience"].Returns("YourAudience");

        _tokenGenerator = new TokenGenerator(_configMock);
    }

    [Test]
    public void GenerateToken_Should_Return_Valid_JWT()
    {
        // Arrange
        var userId = "12345";
        var email = "joe@example.com";

        // Act
        var token = _tokenGenerator.GenerateToken(userId, email);

        // Assert
        token.Should().NotBeNullOrEmpty();

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        jwtToken.Should().NotBeNull();
        jwtToken.Claims.Should().ContainSingle(c => c.Type == JwtRegisteredClaimNames.Sub && c.Value == userId);
        jwtToken.Claims.Should().ContainSingle(c => c.Type == JwtRegisteredClaimNames.Email && c.Value == email);
        jwtToken.Issuer.Should().Be("YourIssuer");
        jwtToken.Audiences.Should().Contain("YourAudience");
    }
}