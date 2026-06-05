using Microsoft.AspNetCore.Mvc;
using QuickBite.Domain.Entities;
using QuickBite.Persistence.DbContext;

namespace QuickBite.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AddressesController:ControllerBase
{
    private readonly ApplicationDbContext _context;
    public AddressesController(ApplicationDbContext context)
    {
        _context = context;
    }

    //GET: api/addresses/user/{userId}
    [HttpGet("user/{userId}")]
    public IActionResult GetAddressesByUser(Guid userId)
    {
        var addresses=_context.Addresses
            .Where(a=>a.UserId==userId)
            .ToList();
        return Ok(addresses);
    }

    //GET: api/addresses/{id}
    [HttpGet("{id}")]
    public IActionResult GetAddressById(Guid id)
    {
        var address = _context.Addresses.FirstOrDefault(a=>a.Id==id);
        if(address==null)
        {
            return NotFound(new
            {
                Message="Address not found"
            });
        }
        return Ok(address);
    }

    //POST: api/addresses
    [HttpPost]
    public IActionResult CreateAddress(Address address)
    {
        address.Id=Guid.NewGuid();
        _context.Addresses.Add(address);
        _context.SaveChanges();
        return Ok(new
        {
            Message="Address created successfully"
        });
    }

    //PUT: api/addresses/{id}
    [HttpPut("{id}")]
    public IActionResult UpdateAddress(Guid id, Address updatedAddress)
    {
        var address=_context.Addresses.FirstOrDefault(a=>a.Id==id);
        if(address==null)
        {
            return NotFound(new
            {
                Message="Address not found"
            });
        }
        address.Type=updatedAddress.Type;
        address.AddressLine=updatedAddress.AddressLine;
        address.City=updatedAddress.City;
        address.State=updatedAddress.State;
        address.Pincode=updatedAddress.Pincode;

        _context.SaveChanges();
        return Ok(new
        {
            Message="Address updated successfully"
        });
    }

    //DELETE: api/addresses/{id}
    [HttpDelete("{id}")]
    public IActionResult DeleteAddress(Guid id)
    {
        var address=_context.Addresses.FirstOrDefault(a=>a.Id==id);
        if(address==null)
        {
            return NotFound(new
            {
                Message="Address not found"
            });
        }
        _context.Addresses.Remove(address);
        _context.SaveChanges();
        return Ok(new
        {
            Message="Address deleted successfully"
        });
    }
}   