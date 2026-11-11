# API de Produtos

API REST em .NET 10 com Clean Architecture, domínio orientado a entidades, Repository Pattern, Entity Framework Core e SQLite.

## Camadas

- `Produtos.Domain`: entidades e regras de domínio.
- `Produtos.Application`: casos de uso, contratos e interfaces dos repositórios.
- `Produtos.Infrastructure`: persistência EF Core, hashing de senhas e geração de JWT.
- `Produtos.Api`: controllers, autenticação, autorização e configuração HTTP.

## Executar

1. Instale o SDK do .NET 10.
2. Copie `.env.example` para `.env` na raiz e substitua `Jwt__Key` por um segredo aleatório de pelo menos 32 bytes. Configure também uma senha forte para o administrador inicial.
3. Execute `dotnet run --project Produtos.Api`.

O banco SQLite `produtos.db` é criado automaticamente na primeira execução. As credenciais do administrador são carregadas apenas na inicialização; se a conta já existir, a senha não será redefinida.

## Autenticação e acesso

- `POST /api/auth/register`: cadastro público; cria usuários com papel `User`.
- `POST /api/auth/login`: retorna token JWT.
- Todos os endpoints têm limite global de 1 requisição a cada 2 segundos por endereço IP; excedentes recebem HTTP 429.
- `GET /api/products` e `GET /api/products/{id}`: exigem JWT com papel `User` ou `Admin`.
- `POST /api/products`, `PUT /api/products/{id}` e `DELETE /api/products/{id}`: exigem JWT com papel `Admin`.

Envie o token no cabeçalho `Authorization: Bearer <token>`. Não há endpoint público para elevar privilégios; o primeiro administrador é provisionado por `BootstrapAdmin__Email` e `BootstrapAdmin__Password` no `.env`.

## Exemplo de produto

```json
{
  "name": "Teclado",
  "description": "Teclado mecânico",
  "price": 299.90
}
```
