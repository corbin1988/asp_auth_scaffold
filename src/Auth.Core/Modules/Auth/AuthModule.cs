using Auth.Core.Modules.Auth.Validators;
using FluentValidation;

namespace Auth.Core.Modules.Auth;

public static class AuthModule
{
    public static IServiceCollection AddAuthModule(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<RegisterDtoValidator>();

        return services;
    }
}