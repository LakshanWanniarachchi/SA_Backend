using System;

namespace api.Service.SendMail
{
    public class emailTemplates
    {
        // Template for Auction Completion Email
        public string AuctionCompleteEmail(string reciverName, decimal WinningBid, string auctionTitle, int auctionId)
        {
            return $@"
                    <!DOCTYPE html>
                    <html lang=""en"">
                    <head>
                        <meta charset=""UTF-8"">
                        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                        <title>Auction Complete</title>
                        <style>
                            @media only screen and (max-width: 600px) {{
                                .container {{
                                    width: 100% !important;
                                }}
                                .content {{
                                    padding: 0 20px !important;
                                }}
                            }}
                        </style>
                    </head>
                    <body style=""margin: 0; padding: 0; background-color: #f6f9fc; font-family: Arial, sans-serif; font-size: 16px; color: #484848;"">
                        <table role=""presentation"" cellspacing=""0"" cellpadding=""0"" border=""0"" align=""center"" width=""100%"" style=""max-width: 600px; margin: 0 auto;"">
                            <tr>
                                <td style=""padding: 20px 0; text-align: center;"">
                                    <img src=""/placeholder.svg?height=80&width=300"" width=""300"" height=""80"" alt=""Car Auction Logo"" style=""display: block; margin: 0 auto;"">
                                </td>
                            </tr>
                            <tr>
                                <td style=""background-color: #ffffff; padding: 40px 30px; border-radius: 6px;"">
                                    <h1 style=""color: #2c3e50; font-size: 28px; text-align: center;"">Auction Complete</h1>
                                    <p>Dear {reciverName},</p>
                                    <p>Your auction for <strong>{auctionTitle}</strong> has been completed. The winning bid was <strong>{WinningBid:C}</strong>.</p>
                                    <p>Thank you for participating in our auction platform. We hope you had a great experience!</p>
                                    <p style=""text-align: center;"">
                                        <a href=""https://yourcarauctionsite.com/{auctionId}"" style=""display: inline-block; background-color: #3498db; padding: 12px 30px; color: #ffffff; text-decoration: none;"">View Auction</a>
                                    </p>
                                    <p>Best regards,<br>Car Auction Team</p>
                                </td>
                            </tr>
                        </table>
                    </body>
                    </html>";
        }

        // Template for Bidder Payment Email
        public string BidderPaymentMail(string reciverName, string url, string auctionTitle)
        {
            return $@"
            <!DOCTYPE html>
            <html lang=""en"">
            <head>
                <meta charset=""UTF-8"">
                <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                <title>Car Auction Payment</title>
                <style>
                    @media only screen and (max-width: 600px) {{
                        .container {{
                            width: 100% !important;
                        }}
                        .content {{
                            padding: 0 20px !important;
                        }}
                    }}
                </style>
            </head>
            <body style=""margin: 0; padding: 0; background-color: #f6f9fc; font-family: Arial, sans-serif; font-size: 16px;"">
                <table role=""presentation"" cellspacing=""0"" cellpadding=""0"" border=""0"" align=""center"" width=""100%"" style=""max-width: 600px; margin: 0 auto;"">
                    <tr>
                        <td style=""padding: 20px 0; text-align: center;"">
                            <img src=""/placeholder.svg?height=80&width=300"" width=""300"" height=""80"" alt=""Car Auction Logo"" style=""display: block; margin: 0 auto;"">
                        </td>
                    </tr>
                    <tr>
                        <td style=""background-color: #ffffff; padding: 40px 30px; border-radius: 6px;"">
                            <h1 style=""color: #2c3e50; font-size: 28px; text-align: center;"">Auction Payment</h1>
                            <p>Dear {reciverName},</p>
                            <p>You have won the {auctionTitle} auction.</p>
                            <a href=""{url}"" style=""display: inline-block; background-color: #3498db; padding: 12px 30px; color: #ffffff; text-decoration: none;"">Make Payment</a>
                        </td>
                    </tr>
                </table>
            </body>
            </html>";
        }
    }
}
