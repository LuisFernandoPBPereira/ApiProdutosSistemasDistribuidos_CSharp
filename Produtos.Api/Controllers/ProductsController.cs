using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Produtos.Application.Products;

namespace Produtos.Api.Controllers;

[ApiController]
[Route("api/products")]
[Authorize]
public sealed class ProductsController(ProductService products) : ControllerBase
{
    [HttpGet]
    [Authorize(Policy = "CanReadProducts")]
    [ProducesResponseType<IReadOnlyList<ProductDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<ProductDto>>> GetAll(CancellationToken cancellationToken) =>
        Ok(await products.GetAllAsync(cancellationToken));

    [HttpGet("{id:guid}")]
    [Authorize(Policy = "CanReadProducts")]
    [ProducesResponseType<ProductDto>(StatusCodes.Status200OK)]
    public async Task<ActionResult<ProductDto>> GetById(Guid id, CancellationToken cancellationToken) =>
        Ok(await products.GetByIdAsync(id, cancellationToken));

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType<ProductDto>(StatusCodes.Status201Created)]
    public async Task<ActionResult<ProductDto>> Create(
        [FromBody] ProductRequest request,
        CancellationToken cancellationToken)
    {
        var product = await products.CreateAsync(
            new ProductInput(request.Name, request.Description, request.Price),
            cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType<ProductDto>(StatusCodes.Status200OK)]
    public async Task<ActionResult<ProductDto>> Update(
        Guid id,
        [FromBody] ProductRequest request,
        CancellationToken cancellationToken)
    {
        var product = await products.UpdateAsync(
            id,
            new ProductInput(request.Name, request.Description, request.Price),
            cancellationToken);

        return Ok(product);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await products.DeleteAsync(id, cancellationToken);
        return NoContent();
    }

    public sealed record ProductRequest(
        [Required, StringLength(160, MinimumLength = 1)] string Name,
        [StringLength(2000)] string? Description,
        [Range(typeof(decimal), "0", "9999999999999999")] decimal Price);
}
