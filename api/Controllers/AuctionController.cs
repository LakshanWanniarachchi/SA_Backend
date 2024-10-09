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
    public IActionResult GetAuctions()
    {
        return Ok(_context.Auctions.Include(c => c.Seller).ToList());
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
            SellerId = user.Id,  // Assuming SellerId is an integer
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            StartingBid = dto.StartingBid,
            WinningBid = dto.WinningBid,
            Status = dto.Status
        };

        _context.Auctions.Add(auction);
        await _context.SaveChangesAsync();

        return Ok(auction);
    }
}
