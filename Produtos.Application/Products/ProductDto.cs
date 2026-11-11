namespace Produtos.Application.Products;

public sealed record ProductDto(
    Guid Id,
    string Name,
    string? Description,
    decimal Price,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc);
