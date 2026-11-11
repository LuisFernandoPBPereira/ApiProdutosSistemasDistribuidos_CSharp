using Produtos.Application.Common;
using Produtos.Domain.Entities;

namespace Produtos.Application.Products;

public sealed class ProductService(IProductRepository repository)
{
    public async Task<IReadOnlyList<ProductDto>> GetAllAsync(CancellationToken cancellationToken) =>
        (await repository.GetAllAsync(cancellationToken)).Select(Map).ToList();

    public async Task<ProductDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var product = await repository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException($"Produto '{id}' não foi encontrado.");

        return Map(product);
    }

    public async Task<ProductDto> CreateAsync(ProductInput input, CancellationToken cancellationToken)
    {
        var product = new Product(input.Name, input.Description, input.Price);
        await repository.AddAsync(product, cancellationToken);
        return Map(product);
    }

    public async Task<ProductDto> UpdateAsync(Guid id, ProductInput input, CancellationToken cancellationToken)
    {
        var product = await repository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException($"Produto '{id}' não foi encontrado.");

        product.Update(input.Name, input.Description, input.Price);
        await repository.UpdateAsync(product, cancellationToken);
        return Map(product);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var product = await repository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException($"Produto '{id}' não foi encontrado.");

        await repository.DeleteAsync(product, cancellationToken);
    }

    private static ProductDto Map(Product product) =>
        new(product.Id, product.Name, product.Description, product.Price, product.CreatedAtUtc, product.UpdatedAtUtc);
}
