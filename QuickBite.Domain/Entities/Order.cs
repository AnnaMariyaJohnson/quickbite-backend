namespace QuickBite.Domain.Entities;

public class Order
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public decimal TotalAmount { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; } = null!;

    public Guid AddressId { get; set; }

    public string DeliveryAddress { get; set; } = string.Empty; 
    
    public string Status {get;set;} = "OrderPlaced";

    public ICollection<OrderItem> OrderItems { get; set;} =
        new List<OrderItem>();
}