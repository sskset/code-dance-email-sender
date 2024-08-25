using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeDance.EmailSender
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string to, string subject, string htmlTemplatePath, Dictionary<string, string> data);
    }
}
