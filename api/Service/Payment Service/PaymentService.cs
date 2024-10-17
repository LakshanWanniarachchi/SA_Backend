using Stripe;
using Stripe.Checkout;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Collections.Generic;

public class PaymentService : IPaymentService
{
    private readonly string _secretKey;

    public PaymentService(IConfiguration configuration)
    {
        _secretKey = configuration["Stripe:SecretKey"];
        StripeConfiguration.ApiKey = _secretKey;
    }

    public async Task<Session> CreateCheckoutSession(decimal amount, string currency = "usd", int auctionId = 0)
    {
        var options = new SessionCreateOptions
        {
            PaymentMethodTypes = new List<string> { "card" },
            LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(amount * 100),
                        Currency = currency,
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = $"Auction #{auctionId} Payment",
                            Description = "Payment for Auction #" + auctionId,
                        },
                    },
                    Quantity = 1,
                },
            },
            Mode = "payment",
            SuccessUrl = "https://yourwebsite.com/success?sessionId={CHECKOUT_SESSION_ID}",
            CancelUrl = "https://yourwebsite.com/cancel",
        };

        var service = new SessionService();
        Session session = await service.CreateAsync(options);

        return session;
    }

    public async Task<PaymentIntent> RetrievePaymentIntent(string paymentIntentId)
    {
        var service = new PaymentIntentService();
        return await service.GetAsync(paymentIntentId);
    }
}
