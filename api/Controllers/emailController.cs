using System.Threading.Tasks;
using api.Interface;
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


            return Ok(new { message = "Email sent successfully" });
        }


    }
}
