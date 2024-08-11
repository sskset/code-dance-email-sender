using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeDance.EmailSender;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace CodeDance.EmailSender.SendGrid
{
    public class SendGridEmailSender : IEmailSender
    {
        private readonly EmailSenderConfig _config;
        private readonly ISendGridClient _client;
        private readonly EmailAddress _fromEmail;

        public SendGridEmailSender(EmailSenderConfig config, ISendGridClient client)
        {
            _config = config;
            _client = client;
            _fromEmail = new EmailAddress(_config.SenderEmail, _config.SenderName);
        }

        public async Task BulkSendAsync(string templateId, IEnumerable<string> tos, List<object> dynamicTemplateData)
        {
            var toEmails = tos.Select(x => new EmailAddress(x, x)).ToList();

            var email = MailHelper.CreateMultipleTemplateEmailsToMultipleRecipients(_fromEmail, toEmails, templateId, dynamicTemplateData);

            await _client.SendEmailAsync(email);
        }

        public async Task SendAsync(string templateId, string to, object dynamicTemplateData)
        {
            var toEmail = new EmailAddress(to, to);

            var email = MailHelper.CreateSingleTemplateEmail(_fromEmail, toEmail, templateId, dynamicTemplateData);

            var res = await _client.SendEmailAsync(email);
        }
    }
}