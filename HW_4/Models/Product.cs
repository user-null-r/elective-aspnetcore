namespace HW_4.Models;

public sealed class Product
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public decimal Price { get; set; }
    public List<Order> Orders { get; set; } = [];
}
