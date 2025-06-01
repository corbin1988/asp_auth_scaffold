using Auth.Core.Modules.Auth.Validators;
using Auth.Core.Modules.User.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace Auth.Core.Modules.Auth;

public static class AuthModule
{
    public static IServiceCollection AddAuthModule(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<RegisterDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<LoginDtoValidator>();

        return services;
    }
}