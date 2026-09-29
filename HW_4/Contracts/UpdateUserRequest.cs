using System.ComponentModel.DataAnnotations;

namespace HW_4.Contracts;

public sealed class UpdateUserRequest
{
    [Required, StringLength(50, MinimumLength = 3)]
    public string UserName { get; init; } = string.Empty;

    [Required, EmailAddress, StringLength(254)]
    public string Email { get; init; } = string.Empty;

    [StringLength(100, MinimumLength = 8)]
    public string? Password { get; init; }
}

