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
        public async Task SendTestEmailAsync(string reciverName, string reciverEmail, string reciverMessage)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Lakshan", "lakshanlop@gmail.com"));
                message.To.Add(new MailboxAddress(reciverName, reciverEmail));
                message.Subject = "Test Email using MailKit";

                // Email body
                message.Body = new TextPart("plain")
                {
                    Text = reciverMessage
                };

                // Initialize SmtpClient and connect to Gmail SMTP
                using (var client = new SmtpClient())
                {
                    // Connect to the Gmail SMTP server asynchronously
                    await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);

                    // Authenticate asynchronously with Gmail
                    await client.AuthenticateAsync("lakshanlop@gmail.com", ""); // Replace with your actual app password

                    // Send the message asynchronously
                    await client.SendAsync(message);

                    // Disconnect asynchronously
                    await client.DisconnectAsync(true);
                }

                Console.WriteLine("Email sent successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }
    }
}
