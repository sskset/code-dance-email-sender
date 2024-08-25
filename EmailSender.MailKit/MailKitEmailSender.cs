namespace CodeDance.EmailSender
{
    using MimeKit;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    public class MailKitEmailSender : IEmailSender
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUser;
        private readonly string _smtpPass;
        private readonly string _senderName;

        public MailKitEmailSender(string smtpServer, int smtpPort, string smtpUser, string smtpPass, string senderName)
        {
            _smtpServer = smtpServer;
            _smtpPort = smtpPort;
            _smtpUser = smtpUser;
            _smtpPass = smtpPass;
            _senderName = senderName;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string htmlTemplatePath, Dictionary<string, string> data)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_senderName, _smtpUser));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = subject;

            // Read the HTML template from a file
            var htmlTemplate = await File.ReadAllTextAsync(htmlTemplatePath);

            if (data != null && data.Any())
            {
                foreach (var kv in data)
                {
                    htmlTemplate = htmlTemplate.Replace("{{" + kv.Key + "}}", kv.Value);
                }
            }


            // Optionally, you can use the model to replace placeholders in the HTML template
            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = htmlTemplate
            };

  
            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                // Connect to the SMTP server
                await client.ConnectAsync(_smtpServer, _smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_smtpUser, _smtpPass);

                // Send the email
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }

}
