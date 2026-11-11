using Microsoft.EntityFrameworkCore;
using Produtos.Application.Auth;
using Produtos.Domain.Entities;

namespace Produtos.Infrastructure.Persistence;

public sealed class DatabaseInitializer(AppDbContext dbContext, IPasswordService passwords)
{
    public async Task InitializeAsync(string? adminEmail, string? adminPassword, CancellationToken cancellationToken)
    {
        await dbContext.Database.EnsureCreatedAsync(cancellationToken);

        if (string.IsNullOrWhiteSpace(adminEmail) && string.IsNullOrWhiteSpace(adminPassword))
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(adminEmail) ||
            string.IsNullOrWhiteSpace(adminPassword) ||
            adminPassword.Length < 12)
        {
            throw new InvalidOperationException(
                "Configure BootstrapAdmin__Email e BootstrapAdmin__Password (mínimo 12 caracteres) juntos.");
        }

        var normalizedEmail = adminEmail.Trim().ToLowerInvariant();
        if (await dbContext.Users.AnyAsync(user => user.Email == normalizedEmail, cancellationToken))
        {
            return;
        }

        var admin = new User(normalizedEmail, string.Empty, "Admin");
        admin = new User(normalizedEmail, passwords.Hash(admin, adminPassword), "Admin");
        await dbContext.Users.AddAsync(admin, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
