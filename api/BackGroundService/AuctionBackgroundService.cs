using api.Data;
using api.Service.SendMail;
using Microsoft.EntityFrameworkCore;

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
                    var paymentService = scope.ServiceProvider.GetRequiredService<PaymentService>();  // Resolve PaymentService within the scope

                    var sriLankanTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Sri Lanka Standard Time");
                    var currentTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, sriLankanTimeZone);

                    var expiredAuctions = await context.Auctions
                        .Where(a => a.EndTime <= currentTime && a.Status != "Complete")
                        .ToListAsync();

                    Console.WriteLine($"Expired Auctions Found: {expiredAuctions.Count}");

                    foreach (var auction in expiredAuctions)
                    {
                        Console.WriteLine($"Processing Auction: {auction.AuctionId}");

                        var winningBid = context.Bids
                            .Where(b => b.AuctionId == auction.AuctionId)
                            .OrderByDescending(b => b.CreatedAt)
                            .FirstOrDefault();

                        auction.Status = "Complete";
                        auction.UpdatedAt = DateTime.UtcNow;

                        if (winningBid == null) continue;

                        auction.WinningBid = winningBid.BidAmount;

                        string? email = context.Users.Where(u => u.Id == auction.SellerId).Select(u => u.Email).FirstOrDefault();
                        string? username = context.Users.Where(u => u.Id == auction.SellerId).Select(u => u.Name).FirstOrDefault();

                        await _mailService.SendAuctionCompleteEmailAsync(username, email, winningBid.BidAmount, auction.Brand, auction.AuctionId);

                        decimal amount = winningBid.BidAmount;

                        var session = await paymentService.CreateCheckoutSession(amount, auction.Brand, auction.Model, auction.AuctionId, auction.AuctionImage);

                        Console.WriteLine(session.Url);

                        string? winBidEmail = context.Users.Where(u => u.Id == winningBid.BidderId).Select(u => u.Email).FirstOrDefault();
                        string? winBidUsername = context.Users.Where(u => u.Id == winningBid.BidderId).Select(u => u.Name).FirstOrDefault();

                        Console.WriteLine($"{winBidEmail} + {winBidUsername}");

                        await _mailService.SendBidderPaymentEmailAsync(winBidUsername, winBidEmail, session.Url, auction.Brand);
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
