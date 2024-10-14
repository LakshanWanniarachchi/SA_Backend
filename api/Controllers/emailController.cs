using System.Threading.Tasks;
using api.Service.SendMail;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/email")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> SendEmail()
        {
            MailService sendMail = new MailService();

            // Sending the email asynchronously
            await sendMail.SendTestEmailAsync();

            return Ok(new { message = "Email sent successfully" });
        }
    }
}
