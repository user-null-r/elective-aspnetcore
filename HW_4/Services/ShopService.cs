using HW_4.Contracts;
using HW_4.Data;
using HW_4.Exceptions;
using HW_4.Models;
using Microsoft.EntityFrameworkCore;

namespace HW_4.Services;

public sealed class ShopService(AppDbContext database)
{
    public async Task<ProductResponse> CreateProductAsync(
        CreateProductRequest request, CancellationToken cancellationToken)
    {
        var product = new Product { Id = Guid.NewGuid(), Name = request.Name.Trim(), Price = request.Price };
        database.Products.Add(product);
        await database.SaveChangesAsync(cancellationToken);
        return new ProductResponse(product.Id, product.Name, product.Price);
    }

    public async Task<IReadOnlyList<ProductResponse>> GetProductsAsync(CancellationToken cancellationToken)
    {
        return await database.Products.AsNoTracking()
            .OrderBy(item => item.Name)
            .Select(item => new ProductResponse(item.Id, item.Name, item.Price))
            .ToListAsync(cancellationToken);
    }

    public async Task<OrderResponse> CreateOrderAsync(
        CreateOrderRequest request, CancellationToken cancellationToken)
    {
        if (!await database.Users.AnyAsync(item => item.Id == request.UserId, cancellationToken))
            throw new NotFoundException("Пользователь не найден.");
        var product = await database.Products.SingleOrDefaultAsync(
                item => item.Id == request.ProductId, cancellationToken)
            ?? throw new NotFoundException("Товар не найден.");
        var order = new Order
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            ProductId = product.Id,
            Quantity = request.Quantity,
            CreatedDate = DateTimeOffset.UtcNow
        };
        database.Orders.Add(order);
        await database.SaveChangesAsync(cancellationToken);
        return new OrderResponse(
            order.Id, order.UserId, product.Id, product.Name,
            order.Quantity, product.Price * order.Quantity, order.CreatedDate);
    }

    public async Task<IReadOnlyList<OrderResponse>> GetUserOrdersAsync(
        Guid userId, CancellationToken cancellationToken)
    {
        if (!await database.Users.AnyAsync(item => item.Id == userId, cancellationToken))
            throw new NotFoundException("Пользователь не найден.");
        return await database.Orders.AsNoTracking()
            .Where(item => item.UserId == userId)
            .OrderBy(item => item.CreatedDate)
            .Select(item => new OrderResponse(
                item.Id, item.UserId, item.ProductId, item.Product!.Name,
                item.Quantity, item.Product.Price * item.Quantity, item.CreatedDate))
            .ToListAsync(cancellationToken);
    }
}
