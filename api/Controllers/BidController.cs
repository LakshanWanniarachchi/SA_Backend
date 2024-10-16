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

            if (user == null)
                return NotFound("User not found");

            var bid = new Bid
            {
                AuctionId = dto.AuctionId,
                BidAmount = dto.BidAmount,
                BidderId = user.Id,
                Status = dto.Status,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context.Bids.Add(bid);
            await _context.SaveChangesAsync();

            return Ok(bid);
        }

    }

}
