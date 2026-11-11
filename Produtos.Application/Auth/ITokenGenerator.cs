using Produtos.Domain.Entities;

namespace Produtos.Application.Auth;

public interface ITokenGenerator
{
    AuthResult Generate(User user);
}
