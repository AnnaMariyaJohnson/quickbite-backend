namespace QuickBite.Application.DTOs.Order;

public class OrderItemRequest
{
    public Guid MenuItemId {get; set;}
    public int Quantity {get; set;}
    public decimal Price {get; set;}
}