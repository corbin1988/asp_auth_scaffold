namespace Auth.Core.Modules.User.Entities;

public class UserEntity
{
    private string _passwordHash;
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Email { get; set; } = string.Empty;

    public string PasswordHash
    {
        get => _passwordHash;
        set => _passwordHash = BCrypt.Net.BCrypt.HashPassword(value);
    }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}