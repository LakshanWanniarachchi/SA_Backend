using Stripe;
using Stripe.Checkout;
using System.Threading.Tasks;

public interface IPaymentService
{
    Task<Session> CreateCheckoutSession(decimal amount, string currency = "usd", int auctionId = 0);
    Task<PaymentIntent> RetrievePaymentIntent(string paymentIntentId);
}