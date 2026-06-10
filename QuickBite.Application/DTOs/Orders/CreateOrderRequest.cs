namespace QuickBite.Application.DTOs.Order;

public class CreateOrderRequest
{
    public decimal TotalAmount { get; set; }

    public Guid AddressId { get; set; }

    public string DeliveryAddress { get; set; } = string.Empty;

    public List<OrderItemRequest> Items { get; set; }
        = new();
}