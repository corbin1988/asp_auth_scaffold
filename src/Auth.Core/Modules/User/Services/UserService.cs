using Auth.Core.Modules.Auth.DTOs;
using Auth.Core.Modules.Shared.JWT;
using Auth.Core.Modules.User.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace Auth.Core.Modules.User.Services;

public class UserService(IUserRepository userRepository, TokenGenerator tokenGenerator, IPasswordHasher<UserEntity> passwordHasher)
    : IUserService
{
    public async Task CreateUserAsync(RegisterDto dto)
    {
        var user = new UserEntity
        {
            Email = dto.Email,
        };
        
        user.PasswordHash = passwordHasher.HashPassword(user, dto.Password);

        await userRepository.CreateUserAsync(user);
    }

    public async Task<string?> LoginAsync(LoginDto dto)
    {
        var user = await userRepository.GetUserByEmailAsync(dto.Email);
        if (user == null) return null;

        var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);

        return result == PasswordVerificationResult.Failed ? null : tokenGenerator.GenerateToken(user.Id.ToString(), user.Email);
    }
}