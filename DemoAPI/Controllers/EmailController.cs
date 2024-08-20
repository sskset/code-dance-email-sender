using System.Threading.Tasks;
using CodeDance.EmailSender;
using EmailSender.SendGrid;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DemoAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class EmailController : ControllerBase
{
    private readonly ILogger<EmailController> _logger;
    private readonly IEmailSender _emailSender;

    public EmailController(ILogger<EmailController> logger, IEmailSender emailSender)
    {
        _logger = logger;
        _emailSender = emailSender;
    }

    [HttpPost(Name = "SendTestEmail")]
    public async Task<IActionResult> SendEmail()
    {
        await _emailSender.SendAsync(EmailTemplate.ConfirmEmail, "sskset+localhost@gmail.com", new
        {
            email = "sskset+localhost@gmail.com",
            emailConfirmationLink = "https://codedance.com.au"
        });

        return Ok();
    }
}
