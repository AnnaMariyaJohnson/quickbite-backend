namespace QuickBite.Domain.Entities;
public class Restaurant
{
    public Guid Id {get; set;}
    public string Name {get; set;}=string.Empty;
    public string Address {get; set;}=string.Empty;
    public string Description {get; set;}=string.Empty;
    public string ImageUrl {get; set;}=string.Empty;
    public double Rating {get; set;}
    public ICollection<MenuItem>MenuItems{get; set;}
    = new List<MenuItem>();
}