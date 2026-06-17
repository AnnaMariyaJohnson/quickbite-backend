using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuickBite.Application.DTOs.Review;
using QuickBite.Domain.Entities;
using QuickBite.Persistence.DbContext;
using System.Security.Claims;

namespace QuickBite.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewsController: ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ReviewsController(
        ApplicationDbContext context)
        {
            _context = context;
        }

    [Authorize]
    [HttpPost]
    public IActionResult AddReview(CreateReviewRequest request)
    {
        var userId = Guid.Parse(
            User.FindFirst(
                ClaimTypes.NameIdentifier
            )!.Value
        );

        if(request.Rating<1 || request.Rating >5)
        {
            return BadRequest(new
            {
                Message="Rating must be between 1 and 5"
            });
        }

        var existingReview=_context.Reviews
            .FirstOrDeafault(r=>
                r.UserId==userId &&
                r.RestaurantId==request.RestaurantId);
        if(existingReiew != null)
        {
            return BadRequest(new
            {
                Message="You have already reviewed this restaurant"
            });
        }

        var review = new Review
        {
            Id = Guid.NewGuid(),
            UserId=userId,
            RestaurantId=request.RestaurantId,
            Rating=request.Rating,
            Comment=request.Comment,
            CreatedAt=DateTime.UtcNow
        };

        _context.Reviews.Add(review);
        _context.SaveChanges();

        return Ok(review);
    }

    [HttpGet("restaurant/{restaurantId}")]
    public IActionResult GetReviews(
        Guid restaurantId)
    {
        var reviews = _context.Reviews
            .Include(r => r.User)
            .Where(r =>
                r.RestaurantId == restaurantId)
            .OrderByDescending(r =>
                r.CreatedAt)
            .Select(r => new
            {
                r.Id,
                r.Rating,
                r.Comment,
                r.CreatedAt,
                UserName = r.User!.Name
            })
            .ToList();

        return Ok(reviews);
    }

    [HttpGet("restaurant/{restaurantId}/average")]
    public IActionResult GetAverageRating(
        Guid restaurantId)
    {
        var reviews = _context.Reviews
            .Where(r=>r.RestaurantId == restaurantId);
        
        var average=reviews
            .Average(r=>(double?)r.Rating) ?? 0;
        var count = reviews.Count();

        return Ok(new
        {
            AverageRating = Math.Round(average,1),
            ReviewsCount=count
        });
    }
}