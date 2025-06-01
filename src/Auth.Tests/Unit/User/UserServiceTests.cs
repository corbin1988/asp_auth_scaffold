using Auth.Core.Modules.Auth.DTOs;
using Auth.Core.Modules.Shared.JWT;
using Auth.Core.Modules.User;
using Auth.Core.Modules.User.Entities;
using Auth.Core.Modules.User.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using NSubstitute;

namespace Auth.Tests.Unit.User;

[TestFixture]
public class UserServiceTests
{
    private IUserRepository _mockRepository = null!;
    private IPasswordHasher<UserEntity> _mockPasswordHasher = null!;
    private IConfiguration _mockConfiguration = null!;
    private TokenGenerator _mockTokenGenerator = null!;
    private UserService _userService = null!;

    [SetUp]
    public void SetUp()
    {
        _mockRepository = Substitute.For<IUserRepository>();
        _mockPasswordHasher = Substitute.For<IPasswordHasher<UserEntity>>();
        _mockConfiguration = Substitute.For<IConfiguration>();
        _mockConfiguration["Jwt:Key"]
            .Returns("a_secure_key_for_testing_a_secure_key_for_testing_a_secure_key_for_testing");
        _mockTokenGenerator = new TokenGenerator(_mockConfiguration);
        _userService = new UserService(_mockRepository, _mockTokenGenerator, _mockPasswordHasher);
    }

    [Test]
    public async Task CreateUserAsync_Should_Call_Repository_With_Correct_Email()
    {
        // Arrange
        var dto = new RegisterDto { Email = "test@example.com", Password = "password123" };

        // Act
        await _userService.CreateUserAsync(dto);

        // Assert
        await _mockRepository.Received(1).CreateUserAsync(Arg.Is<UserEntity>(u => u.Email == dto.Email));
    }

    [Test]
    public async Task LoginAsync_Should_Return_Null_For_Invalid_Credentials()
    {
        // Arrange
        var dto = new LoginDto { Email = "invalid@example.com", Password = "wrongpassword" };
        _mockRepository.GetUserByEmailAsync(dto.Email).Returns((UserEntity?)null);

        // Act
        var result = await _userService.LoginAsync(dto);

        // Assert
        result.Should().BeNull();
    }

    [Test]
    public async Task LoginAsync_Should_Return_Token_For_Valid_Credentials()
    {
        // Arrange
        var dto = new LoginDto { Email = "valid@example.com", Password = "correctpassword" };
        var user = new UserEntity { Id = Guid.NewGuid(), Email = dto.Email, PasswordHash = "hashedpassword" };

        _mockRepository.GetUserByEmailAsync(dto.Email).Returns(user);
        _mockPasswordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password)
            .Returns(PasswordVerificationResult.Success);

        // Act
        var result = await _userService.LoginAsync(dto);

        // Assert
        result.Should().NotBeNullOrEmpty();
    }
}