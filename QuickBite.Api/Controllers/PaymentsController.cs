using Microsoft.AspNetCore.Mvc;
using QuickBite.Application.DTOs.Payment;
using QuickBite.Domain.Entities;
using QuickBite.Persistence.DbContext;
using Razorpay.Api;

namespace QuickBite.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ApplicationDbContext _context;

    public PaymentsController(
        IConfiguration configuration,
        ApplicationDbContext context)
    {
        _configuration = configuration;
        _context = context;
    }

    [HttpPost("create-order")]
    public IActionResult CreateOrder(
        CreatePaymentOrderRequest request)
    {
        var key =
            _configuration["Razorpay:Key"];

        var secret =
            _configuration["Razorpay:Secret"];

        var client =
            new RazorpayClient(
                key,
                secret);

        Dictionary<string, object> options =
            new();

        options.Add(
            "amount",
            request.Amount * 100);

        options.Add(
            "currency",
            "INR");

        options.Add(
            "receipt",
            Guid.NewGuid().ToString());

        var order =
            client.Order.Create(options);

        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            Amount = request.Amount,
            RazorpayOrderId =
                order["id"].ToString()!,
            Status = "Pending",
            CreatedAt = DateTime.UtcNow
        };

        _context.Payments.Add(payment);
        _context.SaveChanges();

        return Ok(new
        {
            Id = order["id"].ToString(),
            Amount = order["amount"].ToString(),
            Currency = order["currency"].ToString(),
            Receipt = order["receipt"].ToString()
        });
    }

    [HttpPost("verify")]
    public IActionResult VerifyPayment(
        VerifyPaymentRequest request)
    {
        var payment = _context.Payments
            .FirstOrDefault(x =>
                x.RazorpayOrderId ==
                request.RazorpayOrderId);

        if(payment == null)
        {
            return NotFound();
        }

        payment.RazorpayPaymentId =
            request.RazorpayPaymentId;

        payment.Status = "Success";

        _context.SaveChanges();

        return Ok(new
        {
            Message = "Payment verified"
        });
    }
}