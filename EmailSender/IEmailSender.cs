using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeDance.EmailSender
{
    public interface IEmailSender
    {
        Task SendAsync(string templateId, string to, object dynamicTemplateData);

        Task BulkSendAsync(string templateId, IEnumerable<string> tos, List<object> dynamicTemplateData);
    }
}