public class Payment
{
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }

    public string RazorpayOrderId { get; set; } = string.Empty;

    public string RazorpayPaymentId { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public string Status { get; set; } = "Pending";

    public DateTime CreatedAt { get; set; }
}