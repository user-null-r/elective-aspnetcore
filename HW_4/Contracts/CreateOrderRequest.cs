using System.ComponentModel.DataAnnotations;

namespace HW_4.Contracts;

public sealed class CreateOrderRequest
{
    public Guid UserId { get; init; }
    public Guid ProductId { get; init; }

    [Range(1, 1000)]
    public int Quantity { get; init; }
}
