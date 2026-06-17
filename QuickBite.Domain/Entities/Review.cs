namespace QuickBite.Domain.Entities;
public class Review
{
    public Guid Id {get;set;}
    public Guid UserId {get; set;}
    public Guid RestaurantId{get; set;}
    public int Rating{get; set;}
    public string Comment{get; set;}=string.Empty;
    public DateTime CreatedAt {get; set;}
    public User ? User {get; set;}
    public Restaurant? Restaurant {get; set;}
}