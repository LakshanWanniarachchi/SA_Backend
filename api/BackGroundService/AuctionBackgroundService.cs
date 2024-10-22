using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using api.Data;
using api.Service.SendMail;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace api.Service.BackgroundTasks
{
    public class AuctionBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly MailService _mailService;



        public AuctionBackgroundService(IServiceProvider serviceProvider, MailService mailService)
        {
            _serviceProvider = serviceProvider;
            _mailService = mailService;


        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await CheckAndProcessExpiredAuctions();
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

        private async Task CheckAndProcessExpiredAuctions()
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    var currentTime = DateTime.UtcNow;

                    var expiredAuctions = await context.Auctions
                        .Where(a => a.EndTime <= currentTime && a.Status != "Complete")
                        .ToListAsync();



                    // Log the number of auctions found
                    Console.WriteLine($"Expired Auctions Found: {expiredAuctions.Count}");

                    foreach (var auction in expiredAuctions)
                    {
                        Console.WriteLine($"Processing Auction: {auction.AuctionId}");

                        var winingbid = context.Bids
                       .Where(b => b.AuctionId == auction.AuctionId)
                       .OrderByDescending(b => b.CreatedAt)
                       .FirstOrDefault();

                        auction.Status = "Complete";
                        auction.UpdatedAt = DateTime.UtcNow;
                        auction.WinningBid = winingbid.BidAmount;


                        string? email = context.Users.Where(u => u.Id == auction.SellerId).Select(u => u.Email).FirstOrDefault();
                        string? username = context.Users.Where(u => u.Id == auction.SellerId).Select(u => u.Name).FirstOrDefault();

                        await _mailService.SendAuctionCompleteEmailAsync(username, email, winingbid.BidAmount, auction.Title, auction.AuctionId);

                        Console.WriteLine(email);


                    }

                    if (expiredAuctions.Any())
                    {
                        await context.SaveChangesAsync();

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AuctionBackgroundService: {ex.Message}");
            }
        }
    }
}