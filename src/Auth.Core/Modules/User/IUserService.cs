using Auth.Core.Modules.Auth.DTOs;

namespace Auth.Core.Modules.User;

public interface IUserService
{
    Task CreateUserAsync(RegisterDto dto);
}