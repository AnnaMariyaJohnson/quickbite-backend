using Microsoft.AspNetCore.Mvc;
using QuickBite.Domain.Entities;
using QuickBite.Persistence.DbContext;
using System.Linq;

namespace QuickBite.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationsController:ControllerBase
{
    private readonly ApplicationDbContext _context;
    public NotificationsController(
        ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("user/{userId}")]
        public IActionResult GetNotifications(Guid userId)
        {
            var notifications = _context.Notifications
                .Where(n=>n.UserId == userId)
                .OrderByDescending(n=>n.CreatedAt)
                .ToList();
            
            return Ok(notifications);
        }

        [HttpPut("{id}/read")]
        public IActionResult MarkAsRead(Guid id)
        {
            var notification = _context.Notifications
                .FirstOrDefault(n=>n.Id == id);

            if(notification == null)
                return NotFound();
            
            notification.IsRead = true;
            _context.SaveChanges();
            return Ok();
        }

        [HttpGet("user/{userId}/unread-count")]
        public IActionResult GetUnreadCount(Guid userId)
        {
            var count = _context.Notifications
                .Count(n => n.UserId == userId && !n.IsRead);

            return Ok(count);
        }
}