using Auth.Core.Modules.Shared.JWT;
using Auth.Core.Modules.User;
using Auth.Core.Modules.User.Entities;
using Auth.Core.Modules.User.Repositories;
using Auth.Core.Modules.User.Services;
using Microsoft.AspNetCore.Identity;

namespace Auth.Core.Modules.Shared.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUserServices(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<TokenGenerator>();
        services.AddScoped<IPasswordHasher<UserEntity>, PasswordHasher<UserEntity>>();
        return services;
    }
}