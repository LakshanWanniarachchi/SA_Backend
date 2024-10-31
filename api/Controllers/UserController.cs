using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        // Inject UserManager to manage user information
        public UserController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Get a list of all users (protected by [Authorize] attribute)
        [HttpGet]
        public IActionResult GetUsers()
        {
            return Ok(_context.Users.ToList());
        }

        // Get details of the currently authenticated user
        [HttpGet("me"), Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            // Retrieve the current user's ID from the token claims
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized("User ID not found in token.");
            }

            // Fetch the user from the UserManager using the user ID
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Return some details about the user
            return Ok(new
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email
            });
        }




        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = registerDTO.Email,
                    Email = registerDTO.Email,
                    Name = registerDTO.Name,  // Storing the Name
                    Address = registerDTO.Address, // Storing the Address
                    mobileNumber = registerDTO.mobileNumber // Storing the Phone Number
                };

                var result = await _userManager.CreateAsync(user, registerDTO.Password);

                if (result.Succeeded)
                {
                    return Ok(new { message = "User registered successfully" });
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }

            return BadRequest("Invalid data");
        }

    }
}
