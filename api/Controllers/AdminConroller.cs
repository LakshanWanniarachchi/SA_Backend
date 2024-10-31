
using api.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private ApplicationDbContext _context;
        private UserManager<IdentityUser> _userManager;

        public AdminController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        [HttpDelete("deleteUser/{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return Ok(new { message = "User deleted successfully" });
            }
            return BadRequest(new { message = "Error deleting user" });
        }


        [HttpGet("viewAllBids")]
        public IActionResult ViewAllBids()
        {
            // Assuming you have a service or context to get the bids from
            var bids = _context.Bids.ToList();
            if (bids == null || bids.Count == 0)
            {
                return Ok(null);
            }
            return Ok(bids);
        }
    }
}