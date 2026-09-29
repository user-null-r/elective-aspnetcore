namespace HW_4.Contracts;

public sealed record UserResponse(
    Guid Id,
    string UserName,
    string Email,
    DateTimeOffset CreatedDate,
    DateTimeOffset UpdatedDate);

