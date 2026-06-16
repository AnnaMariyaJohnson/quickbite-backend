namespace QuickBite.Application.DTOs.Review;

public class CreateReviewRequest
{
    public Guid RestaurantId {get; set;}
    public int Rating {get; set;}
    public string Comment {get; set;}=string.Empty;
}