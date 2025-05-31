using Auth.Core.Modules.Auth.DTOs;
using Auth.Core.Modules.Auth.Validators;
using FluentAssertions;

namespace Auth.Tests.Unit.Auth;

[TestFixture]
public class RegisterDtoValidatorTests
{
    [SetUp]
    public void Setup()
    {
        _validator = new RegisterDtoValidator();
    }

    private RegisterDtoValidator _validator = null!;

    [Test]
    public void Validator_Should_Pass_For_Valid_Dto()
    {
        // Arrange
        var dto = new RegisterDto
        {
            Username = "ValidUsername",
            Password = "ValidPassword123"
        };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public void Validator_Should_Fail_For_Invalid_Username()
    {
        // Arrange
        var dto = new RegisterDto
        {
            Username = "", // Invalid username
            Password = "ValidPassword123"
        };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should()
            .ContainSingle(e => e.PropertyName == "Username" && e.ErrorMessage == "Username is required.");
    }

    [Test]
    public void Validator_Should_Fail_For_Invalid_Password()
    {
        // Arrange
        var dto = new RegisterDto
        {
            Username = "ValidUsername",
            Password = "123" // Invalid password
        };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e =>
            e.PropertyName == "Password" && e.ErrorMessage == "Password must be between 6 and 100 characters.");
    }
}