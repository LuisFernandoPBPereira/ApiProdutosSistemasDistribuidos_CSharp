namespace Produtos.Domain.Entities;

public sealed class User
{
    private User()
    {
    }

    public User(string email, string passwordHash, string role = "User")
    {
        Id = Guid.NewGuid();
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string Role { get; private set; } = "User";
    public DateTime CreatedAtUtc { get; private set; }
}
