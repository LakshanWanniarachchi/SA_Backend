using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Bids;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{


    [Route("api/bids")]
    [ApiController]
    public class BidController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public BidController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {

            _context = context;
            _userManager = userManager;
        }


        [HttpGet, Authorize]
        public IActionResult GetBids()
        {
            return Ok(_context.Bids.ToList());
        }

        [HttpPost("create"), Authorize]
        public async Task<IActionResult> CreateBid([FromBody] CreateBidDto dto)
        {
            // Get the user making the bid
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound("User not found");

            // Retrieve the auction
            var auction = await _context.Auctions.FindAsync(dto.AuctionId);

            if (auction == null)
                return NotFound("Auction not found");

            // Check if auction is already completed
            if (auction.Status == "Complete")
                return Ok("Auction is already complete");

            // Retrieve the last bid for the auction
            var lastBid = _context.Bids
                .Where(b => b.AuctionId == dto.AuctionId)
                .OrderByDescending(b => b.CreatedAt)
                .FirstOrDefault();

            // Ensure that the new bid is greater than the starting bid (if no previous bids exist)
            if (lastBid == null && dto.BidAmount <= auction.StartingBid)
                return BadRequest("Bid amount must be greater than the starting bid");

            // Ensure that the new bid is greater than the last bid
            if (lastBid != null && dto.BidAmount <= lastBid.BidAmount)
                return BadRequest("Bid amount must be greater than the last bid");

            // Create a new bid
            var bid = new Bid
            {
                AuctionId = dto.AuctionId,
                BidAmount = dto.BidAmount,
                BidderId = user.Id,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context.Bids.Add(bid);
            await _context.SaveChangesAsync();

            return Ok(bid);
        }



        [HttpGet("max-bid/{auctionId}"), Authorize]
        public IActionResult GetMaxBid(int auctionId)
        {
            var maxBid = _context.Bids
            .Where(b => b.AuctionId == auctionId)
            .OrderByDescending(b => b.BidAmount)
            .FirstOrDefault();

            if (maxBid == null)
                return NotFound("No bids found for this auction");

            return Ok(maxBid.BidAmount);
        }








    }





}
