namespace QuickBite.Domain.Entities;

public class SupportTicket
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string Subject { get; set; }  = string.Empty;

    public string Message { get; set; } = string.Empty;

    public string Status { get; set; } = "Open";

    public DateTime CreatedAt { get; set; }

    public User? User { get; set; }
}