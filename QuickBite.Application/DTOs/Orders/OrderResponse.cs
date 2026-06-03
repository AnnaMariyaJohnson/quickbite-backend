namespace QuickBite.Application.DTOs.Order;

public class OrderResponse
{
    public Guid Id { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime OrderDate { get; set; }
}