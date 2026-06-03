using Microsoft.AspNetCore.Mvc;
using QuickBite.Domain.Entities;
using QuickBite.Persistence.DbContext;

namespace QuickBite.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MenuController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public MenuController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Menu
    [HttpGet]
    public IActionResult GetAll()
    {
        var items = _context.MenuItems.ToList();
        return Ok(items);
    }

    // GET: api/Menu/{id}
    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        var item = _context.MenuItems.FirstOrDefault(x => x.Id == id);

        if (item == null)
        {
            return NotFound(new
            {
                Message = "Menu item not found"
            });
        }

        return Ok(item);
    }

    // GET: api/Menu/restaurant/{restaurantId}
    [HttpGet("restaurant/{restaurantId}")]
    public IActionResult GetByRestaurant(Guid restaurantId)
    {
        var items = _context.MenuItems
            .Where(x => x.RestaurantId == restaurantId)
            .ToList();

        return Ok(items);
    }

    // POST: api/Menu
    [HttpPost]
    public IActionResult Create(MenuItem item)
    {
        item.Id = Guid.NewGuid();

        _context.MenuItems.Add(item);
        _context.SaveChanges();

        return Ok(new
        {
            Message = "Menu item created successfully"
        });
    }

    // PUT: api/Menu/{id}
    [HttpPut("{id}")]
    public IActionResult Update(Guid id, MenuItem updatedItem)
    {
        var item = _context.MenuItems.FirstOrDefault(x => x.Id == id);

        if (item == null)
        {
            return NotFound(new
            {
                Message = "Menu item not found"
            });
        }

        item.Name = updatedItem.Name;
        item.Price = updatedItem.Price;
        item.Description = updatedItem.Description;
        item.ImageUrl = updatedItem.ImageUrl;
        item.IsVeg = updatedItem.IsVeg;
        item.Category = updatedItem.Category;

        _context.SaveChanges();

        return Ok(new
        {
            Message = "Menu item updated successfully"
        });
    }

    // DELETE: api/Menu/{id}
    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        var item = _context.MenuItems.FirstOrDefault(x => x.Id == id);

        if (item == null)
        {
            return NotFound(new
            {
                Message = "Menu item not found"
            });
        }

        _context.MenuItems.Remove(item);
        _context.SaveChanges();

        return Ok(new
        {
            Message = "Menu item deleted successfully"
        });
    }
}