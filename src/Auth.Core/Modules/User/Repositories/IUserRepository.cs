using Auth.Core.Modules.User.Entities;

namespace Auth.Core.Modules.User;

public interface IUserRepository
{
    Task CreateUserAsync(UserEntity user);
    Task<UserEntity?> GetUserByEmailAsync(string email);
}