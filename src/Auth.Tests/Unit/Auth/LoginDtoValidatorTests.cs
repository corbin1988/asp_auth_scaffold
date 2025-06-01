using Auth.Core.Modules.Auth.DTOs;
using Auth.Core.Modules.Auth.Validators;
using FluentAssertions;

namespace Auth.Tests.Unit.Auth;

[TestFixture]
public class LoginDtoValidatorTests
{
    private LoginDtoValidator _validator = null!;

    [SetUp]
    public void Setup()
    {
        _validator = new LoginDtoValidator();
    }

    [Test]
    public void Validate_Should_Fail_When_Email_Is_Empty()
    {
        // Arrange
        var dto = new LoginDto { Email = string.Empty, Password = "ValidPassword123" };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == "Email" && e.ErrorMessage == "Email is required.");
    }

    [Test]
    public void Validate_Should_Fail_When_Email_Is_Invalid()
    {
        // Arrange
        var dto = new LoginDto { Email = "invalid-email", Password = "ValidPassword123" };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == "Email" && e.ErrorMessage == "Invalid email format.");
    }

    [Test]
    public void Validate_Should_Fail_When_Password_Is_Empty()
    {
        // Arrange
        var dto = new LoginDto { Email = "joe@example.com", Password = string.Empty };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == "Password" && e.ErrorMessage == "Password is required.");
    }

    [Test]
    public void Validate_Should_Pass_When_Email_And_Password_Are_Valid()
    {
        // Arrange
        var dto = new LoginDto { Email = "joe@example.com", Password = "ValidPassword123" };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }
}