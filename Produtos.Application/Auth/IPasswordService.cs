using Produtos.Domain.Entities;

namespace Produtos.Application.Auth;

public interface IPasswordService
{
    string Hash(User user, string password);
    bool Verify(User user, string passwordHash, string password);
}
