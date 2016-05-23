using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using SFA.DAS.NotificationService.Application.Interfaces;

namespace SFA.DAS.NotificationService.Worker
{
    public class LocalEmailService : IEmailService
    {
        public void Send(Dictionary<string, string> items)
        {
            var mail = new MailMessage(GetItemFromInput(items, "fromEmail"), GetItemFromInput(items, "toEmail"));
            var client = new SmtpClient
            {
                Port = 25,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Host = "localhost"
            };
            mail.Subject = GetItemFromInput(items, "subject");
            mail.Body = GetItemFromInput(items, "body");
            client.Send(mail);
        }

        private string GetItemFromInput(Dictionary<string, string> items, string name)
        {
            foreach (var item in items.Where(item => string.Equals(name, item.Key, StringComparison.CurrentCultureIgnoreCase)))
            {
                return item.Value;
            }

            return string.Empty;
        }
    }
}