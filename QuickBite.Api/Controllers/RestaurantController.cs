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
        restaurant.ImageUrl=updatedRestaurant.ImageUrl;
        restaurant.Rating=updatedRestaurant.Rating;
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

    //GET: api/restaurant/search-all?query=pizza
    [HttpGet("search-all")]
    public IActionResult SearchAll(string query)
    {
        if(string.IsNullOrWhiteSpace(query))
        {
            return Ok(new List<object>());
        }

        var restaurantResults = _context.Restaurants
            .Where(r=>
                r.Name.Contains(query) ||
                r.Description.Contains(query))
            .Select(r=>new
            {
                Type="restaurant",
                Restaurant = new
                {
                    r.Id,
                    r.Name,
                    r.Address,
                    r.Description,
                    r.ImageUrl,
                    r.Rating,
                }
            })
            .ToList();
        
        var menuResults = _context.MenuItems
            .Include(m=>m.Restaurant)
            .Where(m=>
                m.Name.Contains(query) ||
                m.Description.Contains(query))
            .Select(m=>new
            {
                Type="dish",
                Restaurant=new
                {
                    m.Restaurant.Id,
                    m.Restaurant.Name,
                    m.Restaurant.Address,
                    m.Restaurant.Description,
                    m.Restaurant.ImageUrl,
                    m.Restaurant.Rating,
                },
                Dish=new
                {
                    m.Id,
                    m.Name,
                    m.Description,
                    m.Price,
                    m.ImageUrl,
                    m.Category,
                    m.IsVeg
                }
            });

        var results=restaurantResults
            .Cast<object>()
            .Concat(menuResults.Cast<object>())
            .ToList();

        return Ok(results);
    }

    //GET: api/restaurant/category/pizza
    [HttpGet("category/{category}")]
    public IActionResult GetRestaurantByCategory(string category)
    {
       var restaurants = _context.Restaurants
        .Include(r => r.MenuItems)
        .Where(r => r.MenuItems.Any(m => m.Category == category))
        .Select(r=>new
        {
            r.Id,
            r.Name,
            r.Address,
            r.Description,
            r.ImageUrl,
            r.Rating,
        })
        .ToList();
        return Ok(restaurants);
    }
}