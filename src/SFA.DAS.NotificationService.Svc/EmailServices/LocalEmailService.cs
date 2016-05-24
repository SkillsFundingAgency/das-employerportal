﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using SFA.DAS.Configuration;
using SFA.DAS.NotificationService.Application;
using SFA.DAS.NotificationService.Application.Interfaces;

namespace SFA.DAS.NotificationService.Worker.EmailServices
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

        public void Send(Dictionary<string, string> items)
        {
            var config = _configurationService.Get<NotificationServiceConfiguration>().Result;

            var mail = new MailMessage(GetItemFromInput(items, "fromEmail"), GetItemFromInput(items, "toEmail"));
            var client = new SmtpClient
            {
                Port = GetPortNumber(config.SmtpServer.Port),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Host = config.SmtpServer.ServerName
            };

            if (!string.IsNullOrEmpty(config.SmtpServer.UserName) && !string.IsNullOrEmpty(config.SmtpServer.Password))
            {
                client.Credentials = new System.Net.NetworkCredential(config.SmtpServer.UserName, config.SmtpServer.Password);
            }
            
            mail.Subject = GetItemFromInput(items, "subject");
            mail.Body = GetItemFromInput(items, "body");
            client.Send(mail);
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