using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Senders.Email
{
    public interface IMessageSender
    {
        public Task SendEmailAsync(string toEmail, string subject, string message, bool isMessageHtml = false);
    }
}
