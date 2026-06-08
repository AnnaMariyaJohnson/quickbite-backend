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
        ApplicatioDbContext context)
        {
            _context =context;
        }

        //GET api/favorites/user/{userId}
        [HttpGet("user/{userId}")]
        public IActionResult GetUserFavorites(Guid userId)
        {
            var favorites = _context.Favorites
                .where(f=> f.UserId == userId)
                .ToList();

            return Ok(favorites);
        }

        [HttpGet("restaurants/{userId}")]
        public IActionResult GetFavoriteRestaurants(Guid userId)
        {
            var restaurants = _context.Favorites
                .where(f=>
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

        [HttpGet("menuItems/{userId}")]
        publ;ic IActionResult GetFavoriteMenuItems(Guid userId)
        {
            var menuItems =_context.Favorites
                .where(f=>
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

        //POST api/favorites
        [HttpPost]
        public IActionResult AddFavorite(
            Favorite favorite)
            {
                favorite.Id = guid.NewGuid();
                _context.Favorites.Add(favorite);
                _context.SaveChanges();

                return Ok(new
                {
                    Message = "Added to favorites"
                });
            }

        //DELETE api/favorite/{id}
        [HttpDelete("{id}")]
        public IActionResult RemoveFavorite(Guid id)
        {
            var favorite = _context.Favorites
                .FirstOrDefault(f=>f.Id == id);
            
            if(favorite == null)
            {
                return NotFound(new
                {
                    Message="Favorite not found"
                });
            }

            _context.Favorites.Remove(favorite);
            _context.SaveChanges();

            return Ok(new
            {
                Message="Removed from favorites"
            })
        }
}