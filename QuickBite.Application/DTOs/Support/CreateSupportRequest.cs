namespace QuickBite.Application.DTOs.Support;

public class CreateSupportRequest
{
    public string Subject {get; set;} = string.Empty;
    public string Message {get; set;} = string.Empty;
}