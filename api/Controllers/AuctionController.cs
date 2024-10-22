using api.Data;
using api.Dtos.Auction;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



[Route("api/actions")]
[ApiController]
public class AuctionController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    // Injecting dependencies via the constructor
    public AuctionController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }


    [HttpGet, Authorize]
    public async Task<IActionResult> GetAuctions()
    {
        var currentTime = DateTime.UtcNow;

        // Fetch all auctions that have expired but are still not marked as "Complete"
        var expiredAuctions = await _context.Auctions
            .Where(a => a.EndTime <= currentTime && a.Status != "Complete")
            .ToListAsync();

        // Update the status of those auctions to "Complete"
        foreach (var auction in expiredAuctions)
        {
            auction.Status = "Complete";
            auction.UpdatedAt = DateTime.UtcNow;
        }

        if (expiredAuctions.Any())
        {
            // Save the changes to the database only if any auctions were updated
            await _context.SaveChangesAsync();
        }

        // Return all auctions (with the updated statuses if any were updated)
        var auctions = await _context.Auctions
            .Include(c => c.Seller)
            .Select(a => new
            {
                a.AuctionId,
                a.Title,
                a.Description,
                a.AuctionCategory,
                a.StartTime,
                a.EndTime,
                a.Status,
                a.WinningBid,
                a.SellerId,
                SellerName = a.Seller.UserName,
                TimeRemaining = a.EndTime - currentTime
            }).ToListAsync();

        return Ok(auctions);
    }


    [HttpPost("create"), Authorize]
    public async Task<IActionResult> CreateAuction([FromBody] CreateAuctionDto dto)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return NotFound("User not found");

        var auction = new Auction
        {
            Title = dto.Title,
            Description = dto.Description,
            AuctionImage = dto.AuctionImage,
            AuctionCategory = dto.AuctionCategory,
            StartingBid = dto.StartingBid,
            SellerId = user.Id,  // Assuming SellerId is an integer
            EndTime = dto.EndTime,
            Status = dto.Status
        };

        _context.Auctions.Add(auction);
        await _context.SaveChangesAsync();

        return Ok(auction);
    }




}
