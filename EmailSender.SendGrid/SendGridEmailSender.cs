using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeDance.EmailSender
{
    public class SendGridEmailSender : ITemplateEmailSender
    {
        private readonly EmailSenderConfig _config;
        private readonly ISendGridClient _client;
        private readonly EmailAddress _fromEmail;
        private readonly ILogger<SendGridEmailSender> _logger;

        public SendGridEmailSender(EmailSenderConfig config, ISendGridClient client, ILogger<SendGridEmailSender> logger)
        {
            _config = config;
            _client = client;
            _fromEmail = new EmailAddress(_config.SenderEmail, _config.SenderName);
            _logger = logger;
        }

        public async Task BulkSendAsync(string templateId, IEnumerable<string> tos, List<object> dynamicTemplateData)
        {
            var toEmails = tos.Select(x => new EmailAddress(x, x)).ToList();

            var email = MailHelper.CreateMultipleTemplateEmailsToMultipleRecipients(_fromEmail, toEmails, templateId, dynamicTemplateData);

            var res = await _client.SendEmailAsync(email);

            if(!res.IsSuccessStatusCode)
            {
                _logger.LogError($"Email failed to send with error status code: {res.StatusCode} and message: {await res.Body.ReadAsStringAsync()}");
            }
        }

        public async Task SendAsync(string templateId, string to, object dynamicTemplateData)
        {
            var toEmail = new EmailAddress(to, to);

            var email = MailHelper.CreateSingleTemplateEmail(_fromEmail, toEmail, templateId, dynamicTemplateData);

            var res = await _client.SendEmailAsync(email);

            if (!res.IsSuccessStatusCode)
            {
                _logger.LogError($"Email failed to send with error status code: {res.StatusCode} and message: {await res.Body.ReadAsStringAsync()}");
            }
        }
    }
}