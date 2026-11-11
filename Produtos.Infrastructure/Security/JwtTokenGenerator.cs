using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Produtos.Application.Auth;
using Produtos.Domain.Entities;

namespace Produtos.Infrastructure.Security;

public sealed class JwtTokenGenerator(IOptions<JwtSettings> settings) : ITokenGenerator
{
    private readonly JwtSettings _settings = settings.Value;

    public AuthResult Generate(User user)
    {
        var expiresAt = DateTime.UtcNow.AddMinutes(_settings.ExpirationMinutes);
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key)),
            SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            _settings.Issuer,
            _settings.Audience,
            claims,
            expires: expiresAt,
            signingCredentials: credentials);

        return new AuthResult(new JwtSecurityTokenHandler().WriteToken(token), expiresAt, user.Email, user.Role);
    }
}
