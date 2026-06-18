using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickBite.Application.DTOs.Support;
using QuickBite.Domain.Entities;
using QuickBite.Persistence.DbContext;
using System.Security.Claims;

namespace QuickBite.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SupportController:ControllerBase
{
    private readonly ApplicationDbContext _context;
    public SupportController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public IActionResult CreateTicket(CreateSupportRequest request)
    {
        var userId = Guid.Parse(
            User.FindFirst(
                ClaimTypes.NameIdentifier)!
            .Value);

        var ticket = new SupportTicket 
        {
            Id= Guid.NewGuid(),
            UserId=userId,
            Subject=request.Subject,
            Message=request.Message,
            CreatedAt=DateTime.UtcNow,
            Status="open"
        };
        _context.SupportTickets.Add(ticket);
        _context.SaveChanges();
        return Ok(ticket);
    }

    [HttpGet]
    public IActionResult MyTicket()
    {
        var userId = Guid.Parse(
            User.FindFirst(
                ClaimTypes.NameIdentifier)!
            .Value);
        
        var tickets = _context.SupportTickets
            .Where(x=>x.UserId == userId)
            .OrderByDescending(x=>x.CreatedAt)
            .ToList();

        return Ok(tickets);
    }
}