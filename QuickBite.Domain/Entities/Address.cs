namespace QuickBite.Domain.Entities;

public class Address
{
    public Guid Id{get; set;}
    public  Guid UserId{get; set;}
    public string Type {get; set;}=string.Empty;
    public string AddressLine {get; set;}=string.Empty;
    public string City{get; set;}=string.Empty;
    public string State {get; set;}=string.Empty;
    public string Pincode{get; set;}=string.Empty;
}