namespace Produtos.Application.Auth;

public sealed record RegisterInput(string Email, string Password);
public sealed record LoginInput(string Email, string Password);
public sealed record AuthResult(string AccessToken, DateTime ExpiresAtUtc, string Email, string Role);
