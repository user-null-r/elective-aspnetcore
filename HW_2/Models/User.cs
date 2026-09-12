namespace HW_2.Models;

public sealed class User
{
    public Guid Id { get; init; }
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public DateTimeOffset CreatedDate { get; init; }
    public DateTimeOffset UpdatedDate { get; set; }
}

