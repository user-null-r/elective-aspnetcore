namespace HW_4.Models;

public sealed class Order
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
    public int Quantity { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
}
