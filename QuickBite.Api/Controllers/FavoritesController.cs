using System.Linq;
using Microsoft.AspNetCore.Mvc;
using QuickBite.Domain.Entities;
using QuickBite.Persistence.DbContext;
namespace QuickBite.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FavoritesController: ControllerBase
{
    private readonly ApplicationDbContext _context;
    public FavoritesController(
        ApplicationDbContext context)
        {
            _context =context;
        }

        //GET api/favorites/user/{userId}
        [HttpGet("user/{userId}")]
        public IActionResult GetUserFavorites(Guid userId)
        {
            var favorites = _context.Favorites
                .Where(f=> f.UserId == userId)
                .ToList();

            return Ok(favorites);
        }

        // GET api/favorites/restaurants/{userId}
        [HttpGet("restaurants/{userId}")]
        public IActionResult GetFavoriteRestaurants(Guid userId)
        {
            var restaurants = _context.Favorites
                .Where(f=>
                    f.UserId==userId &&
                    f.RestaurantId !=null)
                .Join(
                    _context.Restaurants,
                    f=> f.RestaurantId,
                    r =>r.Id,
                    (f,r) => new
                    {
                        FavoriteId =f.Id,
                        RestaurantId = r.Id,
                        r.Name,
                        r.ImageUrl,
                        r.Rating,
                        r.Address
                    })
                .ToList();
            return Ok(restaurants);
        }

    // GET api/favorites/menuItems/{userId}
        [HttpGet("menuItems/{userId}")]
        public IActionResult GetFavoriteMenuItems(Guid userId)
        {
            var menuItems =_context.Favorites
                .Where(f=>
                    f.UserId==userId &&
                    f.MenuItemId != null)
                .Join(
                    _context.MenuItems,
                    f=>f.MenuItemId,
                    m => m.Id,
                    (f,m)=>new
                    {
                        FavoriteId =f.Id,
                        MenuItemId =m.Id,
                        m.Name,
                        m.Description,
                        m.Price,
                        m.ImageUrl
                    })
                .ToList();
            return Ok(menuItems);
        }

        // POST api/favorites
        [HttpPost]
        public IActionResult AddFavorite(Favorite favorite)
        {
            if (favorite.UserId == Guid.Empty)
            {
                return BadRequest(new
                {
                    Message = "Invalid UserId"
                });
            }

            if (favorite.RestaurantId == null &&
                favorite.MenuItemId == null)
            {
                return BadRequest(new
                {
                    Message = "RestaurantId or MenuItemId is required"
                });
            }

            var exists = _context.Favorites.Any(f =>
                f.UserId == favorite.UserId &&
                (
                    f.RestaurantId == favorite.RestaurantId ||
                    f.MenuItemId == favorite.MenuItemId
                ));

            if (exists)
            {
                return BadRequest(new
                {
                    Message = "Already in favorites"
                });
            }

            favorite.Id = Guid.NewGuid();

            _context.Favorites.Add(favorite);
            _context.SaveChanges();

            return Ok(new
            {
                Message = "Added to favorites"
            });
        }

        // DELETE api/favorites/{id}
        [HttpDelete("{id}")]
        public IActionResult RemoveFavorite(Guid id)
        {
            var favorite = _context.Favorites
                .FirstOrDefault(f=>f.Id == id);
            
            if(favorite == null)
            {
                return NotFound(new
                {
                    Message = "Favorite not found"
                });
            }

            _context.Favorites.Remove(favorite);
            _context.SaveChanges();

            return Ok(new
            {
                Message = "Removed from favorites"
            });
        }
}