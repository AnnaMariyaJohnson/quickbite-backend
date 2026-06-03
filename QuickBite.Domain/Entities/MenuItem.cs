namespace QuickBite.Domain.Entities;
public class MenuItem
{
    public Guid Id {get; set;}
    public string Name {get; set;}= string.Empty;
    public decimal Price {get; set;}
    public string Description {get; set;}=string.Empty;
    public Guid RestaurantId {get; set;}
    public Restaurant? Restaurant{get; set;}
}