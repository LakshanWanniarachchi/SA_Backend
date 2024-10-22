using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace api.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<ApplicationUser> Users { get; set; } 

        public DbSet<Auction> Auctions { get; set; }
        public DbSet<Bid> Bids { get; set; }
        public DbSet<Payment> Payments { get; set; }
    }
}
