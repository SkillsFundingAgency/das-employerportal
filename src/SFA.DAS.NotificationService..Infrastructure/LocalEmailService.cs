using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SFA.DAS.Configuration;
using SFA.DAS.NotificationService.Application;
using SFA.DAS.NotificationService.Application.Interfaces;
using SFA.DAS.NotificationService.Application.Messages;

namespace SFA.DAS.NotificationService.Infrastructure
{
    public class LocalEmailService : IEmailService
    {
        private readonly IConfigurationService _configurationService;

        public LocalEmailService(IConfigurationService configurationService)
        {
            if (configurationService == null)
                throw new ArgumentNullException(nameof(configurationService));
            _configurationService = configurationService;
        }

        public async Task SendAsync(EmailMessage message)
        {
            var config = await _configurationService.GetAsync<NotificationServiceConfiguration>();

            using (var client = new SmtpClient())
            {
                client.Port = GetPortNumber(config.SmtpServer.Port);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Host = config.SmtpServer.ServerName;

                if (!string.IsNullOrEmpty(config.SmtpServer.UserName) && !string.IsNullOrEmpty(config.SmtpServer.Password))
                {
                    client.Credentials = new System.Net.NetworkCredential(config.SmtpServer.UserName, config.SmtpServer.Password);
                }

                var mail = new MailMessage(message.ReplyToAddress, message.RecipientsAddress)
                {
                    Subject = message.MessageType,
                    Body = JsonConvert.SerializeObject(message)
                };

                await client.SendMailAsync(mail);
            }
        }

        private int GetPortNumber(string candidate)
        {
            var port = 25;

            int.TryParse(candidate, out port);

            return port;
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