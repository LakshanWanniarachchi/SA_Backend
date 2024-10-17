using Microsoft.AspNetCore.Mvc;
using api.Data;
using System.Threading.Tasks;

[Route("api/payments")]
[ApiController]
public class PaymentsController : ControllerBase
{
    private readonly PaymentService _paymentService;
    private readonly ApplicationDbContext _context;

    public PaymentsController(PaymentService paymentService, ApplicationDbContext context)
    {
        _paymentService = paymentService;
        _context = context;
    }

    [HttpPost("create-checkout-session/{auctionId}/{buyerId}")]
    public async Task<IActionResult> CreateCheckoutSession(int auctionId, string buyerId)
    {
        var auction = await _context.Auctions.FindAsync(auctionId);

        decimal amount = _context.Bids.Where(b => b.AuctionId == auctionId).Max(b => b.BidAmount);

        if (auction == null)
        {
            return BadRequest("Auction not found.");
        }

        // Creating a checkout session via the payment service
        var session = await _paymentService.CreateCheckoutSession(amount, "USD", auctionId);

        if (session == null)
        {
            return BadRequest("Failed to create checkout session.");
        }



        // Saving payment information to the database
        var payment = new Payment
        {
            BuyerId = buyerId,
            SellerId = auction.SellerId,
            AuctionId = auction.AuctionId,
            Amount = amount,  // Assuming StartingBid is the payment amount
            PaymentStatus = "Pending",
            PaymentMethod = "Stripe",
            TransactionId = session.Id  // Setting TransactionId from the session
        };

        _context.Payments.Add(payment);
        await _context.SaveChangesAsync();

        return Ok(new { sessionId = session.Id, Url = session.Url });
    }

    [HttpGet("status/{paymentId}")]
    public async Task<IActionResult> GetPaymentStatus(int paymentId)
    {
        var payment = await _context.Payments.FindAsync(paymentId);

        if (payment == null)
        {
            return NotFound("Payment not found.");
        }

        return Ok(new { status = payment.PaymentStatus });
    }
}
