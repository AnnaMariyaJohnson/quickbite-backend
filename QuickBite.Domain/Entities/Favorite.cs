namespace QuickBite.Domain.Entities;

public class Favorite
{
    public Guid Id{get; set;}
    public Guid UserId{get; set;}
    public Guid? RestaurantId{get; set;}
    public Guid? MenuItemId{get; set;}
}