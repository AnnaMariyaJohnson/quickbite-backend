using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickBite.Application.DTOs.Order;
using QuickBite.Domain.Entities;
using QuickBite.Persistence.DbContext;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace QuickBite.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public OrdersController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public IActionResult CreateOrder(CreateOrderRequest request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var order = new Order
        {
            Id = Guid.NewGuid(),
            UserId = Guid.Parse(userId!),
            TotalAmount = request.TotalAmount,
            AddressId = request.AddressId,
            DeliveryAddress = request.DeliveryAddress,
            CreatedAt = DateTime.UtcNow,
            Status = "OrderPlaced"
        };

        _context.Orders.Add(order);

        foreach(var item in request.Items)
        {
            _context.OrderItems.Add(new OrderItem
            {
                Id = Guid.NewGuid(),
                OrderId= order.Id,
                MenuItemId = item.MenuItemId,
                Quantity= item.Quantity,
                Price=item.Price
            });
        }
        _context.SaveChanges();

        return Ok(new
        {
            order.Id,
            order.TotalAmount,
            order.Status,
            order.CreatedAt,
            order.DeliveryAddress,
            order.UserId,
            order.AddressId,
        });
    }

    [HttpGet]
    public IActionResult GetMyOrders()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var orders = _context.Orders
            .Where(x => x.UserId == Guid.Parse(userId!))
            .OrderByDescending(x => x.CreatedAt)
            .ToList();

        return Ok(orders);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        var order = _context.Orders
            .Include(o=>o.OrderItems)
            .ThenInclude(oi=>oi.MenuItem)
            .FirstOrDefault(o => o.Id == id);

        if(order == null)
            return NotFound();

        return Ok(new
        {
            order.Id,
            order.TotalAmount,
            order.Status,
            order.CreatedAt,
            order.DeliveryAddress,
            order.UserId,

            OrderItems = order.OrderItems.Select(x => new
            {
                x.Id,
                x.Quantity,
                x.Price,
                MenuItemName = x.MenuItem?.Name,
                MenuItemImage= x.MenuItem?.ImageUrl
            })
        });
    }

    [Authorize]
    [HttpGet("my-orders")]
    public IActionResult MyOrders()
    {
        var userId = Guid.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value!
        );

        var orders = _context.Orders
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.CreatedAt)
            .ToList();

        return Ok(orders);
    }

    [HttpPut("{id}/status")]
    public IActionResult UpdateOrderStatus(
        Guid id,
        [FromQuery] string status
    )
    {
        var order = _context.Orders
            .FirstOrDefault(o => o.Id == id);

        if (order == null)
        {
            return NotFound(new
            {
                Message = "Order not found"
            });
        }

        var validStatuses=new[]
        {
            "OrderPlaced",
            "Preparing",
            "OutForDelivery",
            "Delivered",
            "Cancelled"
        };
        if(!validStatuses.Contains(status))
        {
            return BadRequest(new
            {
                Message="Invalid status"
            });
        }

        order.Status = status;
        _context.Notifications.Add(
            new Notification
            {
                Id=Guid.NewGuid(),
                UserId=order.UserId,
                Title="Order Update",
                Message=$"Your order is now {status}",
                IsRead=false,
                CreatedAt=DateTime.UtcNow
            }
        );
        _context.SaveChanges();

        return Ok(new
        {
            Message = "Order status updated successfully",
            order.Status
        });
    }
}