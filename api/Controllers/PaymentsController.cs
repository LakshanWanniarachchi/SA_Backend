using Microsoft.AspNetCore.Mvc;
using api.Data;

[Route("api/payments")]
[ApiController]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    private readonly ApplicationDbContext _context;

    public PaymentsController(IPaymentService paymentService, ApplicationDbContext context)
    {
        _paymentService = paymentService;
        _context = context;
    }

    [HttpPost("create-checkout-session/{auctionId}/{buyerId}")]
    public async Task<IActionResult> CreateCheckoutSession(int auctionId, string buyerId)
    {
        var auction = await _context.Auctions.FindAsync(auctionId);

        if (auction == null || auction.WinningBid == null)
        {
            return BadRequest("Auction not found or no winning bid.");
        }

        var payment = new Payment
        {
            BuyerId = buyerId,
            SellerId = auction.SellerId,
            AuctionId = auction.AuctionId,
            Amount = auction.WinningBid.Value,
            PaymentStatus = "Pending",
            PaymentMethod = "Stripe"
        };

        _context.Payments.Add(payment);
        await _context.SaveChangesAsync();

        var session = await _paymentService.CreateCheckoutSession(payment.Amount, "usd", auctionId);

        payment.TransactionId = session.PaymentIntentId;
        await _context.SaveChangesAsync();

        return Ok(new { sessionId = session.Id });
    }

    [HttpGet("status/{paymentId}")]
    public async Task<IActionResult> GetPaymentStatus(int paymentId)
    {
        var payment = await _context.Payments.FindAsync(paymentId);

        if (payment == null)
        {
            return NotFound("Payment not found");
        }

        return Ok(new { status = payment.PaymentStatus });
    }
}