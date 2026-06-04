using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft EntityFrameworkCore;
using System.Security.Claims;
using QuickBite.Application.DTOs;
using QuickBite.Persistence.DbContext;

namespace QuickBite.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController:ControllerBase
{
    private readonly ApplicationDbContext _context;
    public UserController(ApplicationDbContext context)
    {
        _context = context;
    }

    //GET: api/User/Profile
    [HttpGet("profile")]
    public IActionResult GetProfile()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if(userId==null)
        {
            return Unauthorized();
        }
        
        var user = _context.Users.FirstOrDefault(
            x=>x.Id.ToString() == userId
        );
        
        if(user == null)
        {
            return NotFound(new
            {
                Message="User not found"
            });
        }
        
        var profile= new UserProfileDto
        {
            Id=user.Id,
            FullName=user.FullName,
            Email=user.Email,
            Phone=user.Phone
        };
        return Ok(profile);
    }

    //PUT: api/User/profile
    [HttpPut("profile")]
    public IActionResult UpdateProfile(
        UpdateProfileRequest request)
        {
            var userId=User.FindFirst(ClaimTypes.NameIdentifier)?.Vlaue;

            if(userId==null)
            {
                return Unauthorized();
            }

            var user = _context.Users.FirstOrDefault(
                x=>x.Id.ToString() == userId);
            if(user==null)
            {
                return NotFound(new
                {
                    Message="User not found"
                });
            }
            user.FullName=request.FullName;
            user.Phone=request.Phone;
            
            _context.SaveChanges();
            return Ok(new
            {
                Message="Profile updated successfully"
            });
        }
}