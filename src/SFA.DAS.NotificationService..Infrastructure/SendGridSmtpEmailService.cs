using System;
using System.Net.Mail;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SFA.DAS.Configuration;
using SFA.DAS.NotificationService.Application;
using SFA.DAS.NotificationService.Application.Interfaces;
using SFA.DAS.NotificationService.Application.Messages;

namespace SFA.DAS.NotificationService.Infrastructure
{
    public class SendGridSmtpEmailService : IEmailService
    {
        private readonly IConfigurationService _configurationService;

        public SendGridSmtpEmailService(IConfigurationService configurationService)
        {
            if (configurationService == null)
                throw new ArgumentNullException(nameof(configurationService));
            _configurationService = configurationService;
        }

        public async Task SendAsync(EmailMessage message)
        {
            var config = _configurationService.Get<NotificationServiceConfiguration>();

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
            int port;

            return int.TryParse(candidate, out port) ? port : 25;
        }
    }
}