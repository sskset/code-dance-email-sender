using CodeDance.EmailSender;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;

namespace EmailSender.SendGrid.Extensions.DependencyInjection
{
    public static class SendGridEventSenderCollectionExtensions
    {
        public static IServiceCollection AddSendGridEmailSender(this IServiceCollection services, IConfiguration configuration)
        {

            services.Configure<EmailSenderConfig>(configuration.GetSection(nameof(EmailSenderConfig)));
            services.TryAddTransient<ISendGridClient>(sp =>
            {
                var emailSettings = sp.GetService<IOptions<EmailSenderConfig>>();
                return new SendGridClient(emailSettings.Value.ApiKey);
            });
            services.TryAddTransient<ITemplateEmailSender>(sp => {
                var emailSettings = sp.GetService<IOptions<EmailSenderConfig>>();
                return new SendGridEmailSender(emailSettings.Value, sp.GetRequiredService<ISendGridClient>(), sp.GetService<ILogger<SendGridEmailSender>>());
            });

            return services;
        }
    }
}
