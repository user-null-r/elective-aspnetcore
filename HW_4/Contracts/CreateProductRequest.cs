using System.ComponentModel.DataAnnotations;

namespace HW_4.Contracts;

public sealed class CreateProductRequest
{
    [Required, StringLength(100, MinimumLength = 1)]
    public string Name { get; init; } = string.Empty;

    [Range(0.01, 1_000_000)]
    public decimal Price { get; init; }
}
