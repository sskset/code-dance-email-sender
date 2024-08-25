using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeDance.EmailSender
{
    public interface ITemplateEmailSender
    {
        Task SendAsync(string templateId, string to, object dynamicTemplateData);

        Task BulkSendAsync(string templateId, IEnumerable<string> tos, List<object> dynamicTemplateData);
    }
}