using System;
using System.Threading.Tasks;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using api.Interface;

namespace api.Service.SendMail
{
    public class MailService : IEmail
    {
        private readonly emailTemplates _templates;

        public MailService()
        {
            _templates = new emailTemplates();  // Initialize the template class
        }
        public async Task SendAuctionCompleteEmailAsync(string reciverName, string reciverEmail, decimal winningBid, string auctionTitle, int auctionId)
        {
            try
            {
                // Create a new email message
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Car Auction", "wildelephant.info@gmail.com"));
                message.To.Add(new MailboxAddress(reciverName, reciverEmail));
                message.Subject = "Auction Complete - Auction Update";

                // Build the HTML email body using the template
                var builder = new BodyBuilder();
                builder.HtmlBody = _templates.AuctionCompleteEmail(reciverName, winningBid, auctionTitle, auctionId);

                // Attach the HTML body to the email message
                message.Body = builder.ToMessageBody();

                // Initialize the SMTP client and send the email
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync("wildelephant.info@gmail.com", "uukx ipav zcfm oplj"); // Replace with your actual app password
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }

                Console.WriteLine("Auction completion email sent successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }
        public async Task SendBidderPaymentEmailAsync(string reciverName, string reciverEmail, string url, string auctionTitle)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Car Auction", "wildelephant.info@gmail.com"));
                message.To.Add(new MailboxAddress(reciverName, reciverEmail));
                message.Subject = "Auction Payment Required";

                // Build the HTML email using the template
                string emailTemplate = _templates.BidderPaymentMail(reciverName, url, auctionTitle);

                // Attach the HTML body to the email message
                var builder = new BodyBuilder { HtmlBody = emailTemplate };
                message.Body = builder.ToMessageBody();

                // Initialize SmtpClient and connect to Gmail SMTP
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);

                    // Authenticate with the app password
                    await client.AuthenticateAsync("wildelephant.info@gmail.com", "uukx ipav zcfm oplj"); // Replace with actual app password

                    // Send the email
                    await client.SendAsync(message);

                    await client.DisconnectAsync(true);
                }

                Console.WriteLine("Bidder payment email sent successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }

        public Task SendTestEmailAsync(string reciverName, string reciverEmail, string reciverMessage)
        {
            throw new NotImplementedException();
        }
    }
}
