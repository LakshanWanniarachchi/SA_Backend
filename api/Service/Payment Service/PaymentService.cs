using Stripe;
using Stripe.Checkout;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Collections.Generic;

public class PaymentService
{
    private readonly string _secretKey;

    public PaymentService(IConfiguration configuration)
    {
        // _secretKey = configuration["Stripe:SecretKey"];
        StripeConfiguration.ApiKey = "sk_test_51Q9R1Z2KhZ1sCuSZPtPjXnRkgO9cjLZ4vEaSrqOfhl3sNt5Ia43qh0F6VHtNzj9nSwonksHd5Q9RlqMj5c7hqFOl00zzBlIYbG";
    }

    public async Task<Session> CreateCheckoutSession(decimal amount, string brand, string model, int auctionId, string auctionImage)
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
                    UnitAmount = (long)(amount * 100),  // Convert decimal amount to cents
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {

                        Name = $"{brand}-{model} Auction #{auctionId} Payment",
                        Description = "Payment for Auction #" + auctionId,
                        Images = new List<string> { auctionImage },  // Image URL must be HTTPS and accessible
                    },
                },
                Quantity = 1,
            },
        },
            Mode = "payment",
            SuccessUrl = "http://localhost:3000/paymet-success/{CHECKOUT_SESSION_ID}",
            CancelUrl = "http://localhost:5175/cancel",
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