using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuickBite.Domain.Entities;
using QuickBite.Persistence.DbContext;

namespace QuickBite.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RestaurantController:ControllerBase
{
    private readonly ApplicationDbContext _context;
    public RestaurantController(ApplicationDbContext context)
    {
        _context =context;
    }
    //GET:api/restaurant
    [HttpGet]
    public IActionResult GetAllRestaurants()
    {
        var restaurants =_context.Restaurants.ToList();
        return Ok(restaurants);
    }

    //GET: api/restaurant/{id}
    [HttpGet("{id}")]
    public IActionResult GetRestaurantById(Guid id)
    {
        var restaurant=_context.Restaurants.FirstOrDefault(r=>r.Id==id);
        if(restaurant==null)
        {
            return NotFound(new
            {
                Message="Restaurant not found"
            });
        }
        return Ok(restaurant);
    }

    //POST:api/restaurant
    [HttpPost]
    public IActionResult CreateRestaurant(Restaurant restaurant)
    {
        restaurant.Id=Guid.NewGuid();
        _context.Restaurants.Add(restaurant);
        _context.SaveChanges();

        return Ok(new
        {
            Message="Restaurant created successfully"
        });
    }

    //PUT:api/restaurant/{id}
    [HttpPut("{id}")]
    public IActionResult UpdateRestaurant(Guid id,Restaurant updatedRestaurant)
    {
        var restaurant=_context.Restaurants.FirstOrDefault(r=>r.Id==id);
        if(restaurant==null)
        {
            return NotFound(new
            {
                Message="Restaurant not found"
            });
        }

        restaurant.Name=updatedRestaurant.Name;
        restaurant.Address=updatedRestaurant.Address;
        restaurant.Description=updatedRestaurant.Description;
        _context.SaveChanges();
        return Ok(new
        {
            Message="Restaurant updated successfully"
        });
    }

    //DELETE: api/restaurant/{id}
    [HttpDelete("{id}")]
    public IActionResult DeleteRestaurant(Guid id)
    {
        var restaurant=_context.Restaurants.FirstOrDefault(r=>r.Id==id);
        if(restaurant==null)
        {
            return NotFound(new
            {
                Message="Restaurant not found"
            });
        }
        _context.Restaurants.Remove(restaurant);
        _context.SaveChanges();

        return Ok(new
        {
            Message="Restaurant deleted successfully"
        });
    }
}