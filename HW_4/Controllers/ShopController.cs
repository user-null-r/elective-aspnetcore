using HW_4.Contracts;
using HW_4.Services;
using Microsoft.AspNetCore.Mvc;

namespace HW_4.Controllers;

[ApiController]
[Route("api")]
public sealed class ShopController(ShopService shop) : ControllerBase
{
    [HttpPost("products")]
    [ProducesResponseType<ProductResponse>(StatusCodes.Status201Created)]
    public async Task<ActionResult<ProductResponse>> CreateProduct(
        CreateProductRequest request, CancellationToken cancellationToken)
    {
        var result = await shop.CreateProductAsync(request, cancellationToken);
        return Created($"/api/products/{result.Id}", result);
    }

    [HttpGet("products")]
    public async Task<ActionResult<IReadOnlyList<ProductResponse>>> GetProducts(
        CancellationToken cancellationToken)
    {
        return Ok(await shop.GetProductsAsync(cancellationToken));
    }

    [HttpPost("orders")]
    [ProducesResponseType<OrderResponse>(StatusCodes.Status201Created)]
    public async Task<ActionResult<OrderResponse>> CreateOrder(
        CreateOrderRequest request, CancellationToken cancellationToken)
    {
        var result = await shop.CreateOrderAsync(request, cancellationToken);
        return Created($"/api/orders/{result.Id}", result);
    }

    [HttpGet("users/{userId:guid}/orders")]
    public async Task<ActionResult<IReadOnlyList<OrderResponse>>> GetUserOrders(
        Guid userId, CancellationToken cancellationToken)
    {
        return Ok(await shop.GetUserOrdersAsync(userId, cancellationToken));
    }
}
