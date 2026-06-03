using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickBite.Application.DTOs;
using QuickBite.Domain.Entities;
using QuickBite.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using BCrypt.Net;

namespace QuickBite.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController :ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AuthController(ApplicationDbContext context)
    {
        _context =context;
    }

    [HttpPost("register")]
    public IActionResult Register(RegisterRequest request)
    {
        if(_context.Users.Any(x=> x.Email == request.Email))
        {
            return BadRequest(new
            {
                Message="Email already exists"
            });
        }
        var user = new User
        {
            Id=Guid.NewGuid(),
            FullName=request.FullName,
            Email=request.Email,
            PasswordHash=BCrypt.Net.BCrypt.HashPassword(request.Password)
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        return Ok(new
        {
            Message="User Registered Successfully"
        });
    }

    [HttpPost("login")]
    public IActionResult Login(LoginRequest request)
    {
        var user=_context.Users.FirstOrDefault(x=>
        x.Email == request.Email);

        if(user==null)
        {
            return Unauthorized(new
            {
                Message="Invalid Email"
            });
        }
        if(!BCrypt.Net.BCrypt.Verify(
            request.Password,
            user.PasswordHash))
            {
            return Unauthorized(new
            {
                Message="Invalid Password"
            });
        }
        var claims =new[]
        {
            new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
            new Claim(ClaimTypes.Name,user.FullName),
            new Claim(ClaimTypes.Email,user.Email)
        };
        var Key= new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                HttpContext.RequestServices
                    .GetRequiredService<IConfiguration>()["Jwt:Key"]!));
        var creds=new SigningCredentials(
            Key,
            SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer:"QuickBite",
            audience:"QuickBiteUsers",
            claims:claims,
            expires:DateTime.Now.AddHours(2),
            signingCredentials:creds
        );
        return Ok(new
        {
            token= new JwtSecurityTokenHandler().WriteToken(token)
        });
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var fullName = User.FindFirst(ClaimTypes.Name)?.Value;
        var email = User.FindFirst(ClaimTypes.Email)?.Value;

        return Ok(new
        {
            UserId = userId,
            FullName=fullName,
            Email=email
        });
    }
}