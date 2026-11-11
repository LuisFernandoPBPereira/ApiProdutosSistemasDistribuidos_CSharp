using Microsoft.AspNetCore.Identity;
using Produtos.Application.Auth;
using Produtos.Domain.Entities;

namespace Produtos.Infrastructure.Security;

public sealed class AspNetPasswordService : IPasswordService
{
    private readonly PasswordHasher<User> _hasher = new();

    public string Hash(User user, string password) =>
        _hasher.HashPassword(user, password);

    public bool Verify(User user, string passwordHash, string password) =>
        _hasher.VerifyHashedPassword(user, passwordHash, password) != PasswordVerificationResult.Failed;
}
