namespace Produtos.Application.Products;

public sealed record ProductInput(string Name, string? Description, decimal Price);
