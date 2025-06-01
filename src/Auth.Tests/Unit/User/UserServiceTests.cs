using Auth.Core.Modules.Auth.DTOs;
using Auth.Core.Modules.User;
using Auth.Core.Modules.User.Entities;
using Auth.Core.Modules.User.Services;
using NSubstitute;

namespace Auth.Tests.Unit.User;

[TestFixture]
public class UserServiceTests
{
    [Test]
    public async Task CreateUserAsync_Should_Call_Repository_With_Correct_Email()
    {
        // Arrange
        var mockRepository = Substitute.For<IUserRepository>();
        var dto = new RegisterDto { Email = "test@example.com", Password = "password123" };
        var userService = new UserService(mockRepository, null!);

        // Act
        await userService.CreateUserAsync(dto);

        // Assert
        await mockRepository.Received(1).CreateUserAsync(Arg.Is<UserEntity>(u => u.Email == dto.Email));
    }
}