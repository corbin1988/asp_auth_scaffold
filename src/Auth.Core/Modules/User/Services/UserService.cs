using Auth.Core.Modules.Auth.DTOs;
using Auth.Core.Modules.User.Entities;
using FluentValidation;

namespace Auth.Core.Modules.User.Services;

public class UserService(IUserRepository userRepository, IValidator<RegisterDto> validator)
    : IUserService
{
    public async Task CreateUserAsync(RegisterDto dto)
    {
        var user = new UserEntity
        {
            Email = dto.Email,
            PasswordHash = dto.Password
        };

        await userRepository.CreateUserAsync(user);
    }
}