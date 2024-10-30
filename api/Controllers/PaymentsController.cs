using Microsoft.AspNetCore.Mvc;
using api.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

[Route("api/payments")]
[ApiController]
public class PaymentsController : ControllerBase
{
    private readonly PaymentService _paymentService;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public PaymentsController(PaymentService paymentService, ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _paymentService = paymentService;
        _context = context;
        _userManager = userManager;
    }

    [HttpPost("create-checkout-session/{auctionId}")]
    public async Task<IActionResult> CreateCheckoutSession(int auctionId)
    {


        var bid = await _context.Bids
            .Where(b => b.AuctionId == auctionId)
            .OrderByDescending(b => b.BidAmount)
            .FirstOrDefaultAsync();



        var auction = await _context.Auctions.FindAsync(auctionId);

        decimal amount = _context.Bids.Where(b => b.AuctionId == auctionId).Max(b => b.BidAmount);

        if (auction == null)
        {
            return BadRequest("Auction not found.");
        }

        // Creating a checkout session via the payment service
        var session = await _paymentService.CreateCheckoutSession(amount, auction.Brand, auction.Model, auction.AuctionId, auction.AuctionImage);

        if (session == null)
        {
            return BadRequest("Failed to create checkout session.");
        }



        // Saving payment information to the database
        var payment = new Payment
        {
            BuyerId = bid.BidderId,
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

    [HttpPost("complete-auction/{CHECKOUT_SESSION_ID}")]
    public async Task<IActionResult> CompleteAuction(string CHECKOUT_SESSION_ID)
    {

        var payment = await _context.Payments
            .Where(p => p.TransactionId == CHECKOUT_SESSION_ID)
            .FirstOrDefaultAsync();

        if (payment == null)
        {
            return BadRequest("Pending payment not found for this auction.");
        }

        payment.PaymentStatus = "Completed";
        _context.Payments.Update(payment);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Auction completed and payment status updated to Completed." });
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