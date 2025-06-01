using Auth.Core.Modules.Shared.Database;
using Auth.Core.Modules.User.Entities;
using Microsoft.EntityFrameworkCore;

namespace Auth.Core.Modules.User.Repositories;

public class UserRepository(AppDbContext dbContext) : IUserRepository
{
    public async Task CreateUserAsync(UserEntity user)
    {
        await dbContext.Users.AddAsync(user);
        await dbContext.SaveChangesAsync();
    }

    public async Task<UserEntity?> GetUserByEmailAsync(string email)
    {
        return await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
}