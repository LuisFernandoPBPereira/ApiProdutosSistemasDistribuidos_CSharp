namespace Produtos.Domain.Entities;

public sealed class Product
{
    private Product()
    {
    }

    public Product(string name, string? description, decimal price)
    {
        Id = Guid.NewGuid();
        CreatedAtUtc = DateTime.UtcNow;
        Update(name, description, price);
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public decimal Price { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime UpdatedAtUtc { get; private set; }

    public void Update(string name, string? description, decimal price)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("O nome do produto é obrigatório.", nameof(name));
        }

        if (name.Length > 160)
        {
            throw new ArgumentException("O nome do produto deve ter no máximo 160 caracteres.", nameof(name));
        }

        if (description?.Length > 2000)
        {
            throw new ArgumentException("A descrição deve ter no máximo 2000 caracteres.", nameof(description));
        }

        if (price < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(price), "O preço não pode ser negativo.");
        }

        Name = name.Trim();
        Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
        Price = price;
        UpdatedAtUtc = DateTime.UtcNow;
    }
}
