using Auth.Core.Modules.Auth.DTOs;
using Auth.Core.Modules.Auth.Validators;
using Auth.Core.Modules.User;
using Auth.Core.Modules.User.Entities;
using FluentAssertions;
using NSubstitute;

namespace Auth.Tests.Unit.Auth;

[TestFixture]
public class RegisterDtoValidatorTests
{
    [SetUp]
    public void Setup()
    {
        _userMock = Substitute.For<IUserRepository>();
        _validator = new RegisterDtoValidator(_userMock);
    }

    private RegisterDtoValidator _validator = null!;
    private IUserRepository _userMock;

    [Test]
    public async Task Validator_Should_Pass_For_Valid_Dto()
    {
        // Arrange
        var dto = new RegisterDto
        {
            Email = "joe@example.com",
            Password = "ValidPassword123"
        };
        _userMock.GetUserByEmailAsync(dto.Email).Returns((UserEntity?)null);

        // Act
        var result = await _validator.ValidateAsync(dto);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public async Task Validator_Should_Fail_For_Existing_Email()
    {
        // Arrange
        var dto = new RegisterDto
        {
            Email = "existing@example.com",
            Password = "ValidPassword123"
        };
        _userMock.GetUserByEmailAsync(dto.Email).Returns(new UserEntity { Email = dto.Email });

        // Act
        var result = await _validator.ValidateAsync(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e =>
            e.PropertyName == "Email" && e.ErrorMessage == "A user with this email already exists.");
    }

    [Test]
    public async Task Validator_Should_Fail_For_Invalid_Password()
    {
        // Arrange
        var dto = new RegisterDto
        {
            Email = "joe@example.com",
            Password = "123"
        };
        _userMock.GetUserByEmailAsync(dto.Email).Returns((UserEntity?)null);

        // Act
        var result = await _validator.ValidateAsync(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e =>
            e.PropertyName == "Password" && e.ErrorMessage == "Password must be between 6 and 100 characters.");
    }
}