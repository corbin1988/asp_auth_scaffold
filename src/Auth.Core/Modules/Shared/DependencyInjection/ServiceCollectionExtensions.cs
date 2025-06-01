using Auth.Core.Modules.User;
using Auth.Core.Modules.User.Repositories;
using Auth.Core.Modules.User.Services;

namespace Auth.Core.Modules.Shared.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUserServices(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserService, UserService>();
        return services;
    }
}