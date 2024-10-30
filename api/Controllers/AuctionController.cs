using api.Builder_Design_Pattern.AuctionBuilderDesingPattern;
using api.Data;
using api.Dtos.Auction;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SA_Backend.api.Builder_Design_Pattern.AuctionBuilderDesingPattern;



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


    [HttpGet]
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
                a.Brand,
                a.Description,
                a.Model,
                a.Mileage,
                a.StartingBid,
                a.AuctionImage,
                a.StartTime,
                a.EndTime,
                a.Status,
                a.WinningBid,
                a.SellerId,
                SellerName = a.Seller.UserName,
                TimeRemaining = a.EndTime - currentTime,

                CurrentBid = _context.Bids
                .Where(b => b.AuctionId == a.AuctionId)
                .OrderByDescending(b => b.BidAmount)
                .Select(b => (decimal?)b.BidAmount)
                .FirstOrDefault() ?? a.StartingBid


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



        // Vehical auction = new Director(new Car()).construct(dto , user.Id);

        var auction = new Auction
        {
            Brand = dto.Brand,
            Description = dto.Description,
            AuctionImage = dto.AuctionImage,
            Year = dto.Year,
            Model = dto.Model,
            Mileage = dto.Mileage,
            StartingBid = dto.StartingBid,
            SellerId = user.Id,  // Assuming SellerId is an integer
            EndTime = dto.EndTime,

        };

        _context.Auctions.Add(auction);
        await _context.SaveChangesAsync();

        return Ok(auction);
    }


    [HttpDelete("delete/{auctionId}"), Authorize]
    public async Task<IActionResult> DeleteAuction(int auctionId)
    {

        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
            return Unauthorized("User not found");



        var auction = await _context.Auctions.FirstOrDefaultAsync(a => a.AuctionId == auctionId && a.SellerId == userId);



        if (auction == null)
            return NotFound("Auction not found");

        _context.Auctions.Remove(auction);
        await _context.SaveChangesAsync();

        return Ok("Auction deleted successfully");
    }


    [HttpGet("/GetUserAuctions"), Authorize]
    public async Task<IActionResult> GetUserAuctions()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
            return Unauthorized("User not found");

        var auctions = await _context.Auctions
            .Where(a => a.SellerId == userId)
            .Include(c => c.Seller)
            .Select(a => new
            {
                a.AuctionId,
                a.Brand,
                a.Description,
                a.Model,
                a.Mileage,
                a.StartingBid,
                a.AuctionImage,
                a.StartTime,
                a.EndTime,
                a.Status,
                a.WinningBid,
                a.SellerId,
                SellerName = a.Seller.UserName,
                TimeRemaining = a.EndTime - DateTime.UtcNow
            }).ToListAsync();

        return Ok(auctions);
    }

    [HttpPut("update/{auctionId}"), Authorize]
    public async Task<IActionResult> UpdateAuction(int auctionId, [FromBody] UpdateAuctionDto dto)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
            return Unauthorized("User not found");

        var auction = await _context.Auctions.FirstOrDefaultAsync(a => a.AuctionId == auctionId && a.SellerId == userId);

        if (auction == null)
            return NotFound("Auction not found");

        auction.Brand = dto.Brand;
        auction.Description = dto.Description;
        auction.AuctionImage = dto.AuctionImage;
        auction.Year = dto.Year;
        auction.Model = dto.Model;
        auction.Mileage = dto.Mileage;
        auction.StartingBid = dto.StartingBid;
        auction.EndTime = dto.EndTime;
        auction.UpdatedAt = DateTime.UtcNow;

        _context.Auctions.Update(auction);
        await _context.SaveChangesAsync();

        return Ok(auction);
    }



    [HttpGet("{auctionId}")]
    public async Task<IActionResult> GetAuctionById(int auctionId)
    {
        var auction = await _context.Auctions
            .Include(a => a.Seller)
            .Where(a => a.AuctionId == auctionId)
            .Select(a => new
            {
                a.AuctionId,
                a.Brand,
                a.Description,
                a.Model,
                a.Mileage,
                a.StartingBid,
                a.AuctionImage,
                a.StartTime,
                a.EndTime,
                a.Status,
                a.WinningBid,
                a.SellerId,
                SellerName = a.Seller.UserName,
                TimeRemaining = a.EndTime - DateTime.UtcNow,
                CurrentBid = _context.Bids
                    .Where(b => b.AuctionId == a.AuctionId)
                    .OrderByDescending(b => b.BidAmount)
                    .Select(b => (decimal?)b.BidAmount)
                    .FirstOrDefault() ?? a.StartingBid
            })
            .FirstOrDefaultAsync();

        if (auction == null)
            return NotFound("Auction not found");

        return Ok(auction);
    }




}
