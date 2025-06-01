using Auth.Core.Modules.User;
using Auth.Core.Modules.User.Repositories;

namespace Auth.Core.Modules.Shared.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUserServices(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        // services.AddScoped<UserService>();
        return services;
    }
}