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
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId);


            if (_context.Auctions.Find(dto.AuctionId).Status == "Complete")
                return Ok("Auction is already complete");

            var lastBid = _context.Bids
                .Where(b => b.AuctionId == dto.AuctionId)
                .OrderByDescending(b => b.CreatedAt)
                .FirstOrDefault();

            if (lastBid != null && dto.BidAmount <= lastBid.BidAmount)
                return Ok("Bid amount must be greater than the last bid");

            if (user == null)
                return NotFound("User not found");

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

    }

}
