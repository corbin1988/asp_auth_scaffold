using Auth.Core.Modules.Auth.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace Auth.Core.Modules.Auth;

public static class AuthModule
{
    public static IServiceCollection AddAuthModule(this IServiceCollection services)
    {
        services.AddControllers();

        services.AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters();

        services.AddValidatorsFromAssemblyContaining<RegisterDtoValidator>();

        return services;
    }
}