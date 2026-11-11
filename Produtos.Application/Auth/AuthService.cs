using Produtos.Application.Common;
using Produtos.Domain.Entities;

namespace Produtos.Application.Auth;

public sealed class AuthService(
    IUserRepository users,
    IPasswordService passwords,
    ITokenGenerator tokens)
{
    public async Task<AuthResult> RegisterAsync(RegisterInput input, CancellationToken cancellationToken)
    {
        var email = NormalizeEmail(input.Email);
        if (await users.GetByEmailAsync(email, cancellationToken) is not null)
        {
            throw new ConflictException("Já existe uma conta cadastrada com este e-mail.");
        }

        var user = new User(email, string.Empty);
        user = new User(email, passwords.Hash(user, input.Password));
        await users.AddAsync(user, cancellationToken);
        return tokens.Generate(user);
    }

    public async Task<AuthResult> LoginAsync(LoginInput input, CancellationToken cancellationToken)
    {
        var user = await users.GetByEmailAsync(NormalizeEmail(input.Email), cancellationToken);
        if (user is null || !passwords.Verify(user, user.PasswordHash, input.Password))
        {
            throw new UnauthorizedAccessException("E-mail ou senha inválidos.");
        }

        return tokens.Generate(user);
    }

    private static string NormalizeEmail(string email) => email.Trim().ToLowerInvariant();
}
