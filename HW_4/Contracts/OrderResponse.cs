namespace HW_4.Contracts;

public sealed record OrderResponse(
    Guid Id,
    Guid UserId,
    Guid ProductId,
    string ProductName,
    int Quantity,
    decimal Total,
    DateTimeOffset CreatedDate);
