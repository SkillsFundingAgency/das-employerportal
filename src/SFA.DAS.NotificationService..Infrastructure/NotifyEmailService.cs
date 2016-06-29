using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SFA.DAS.Configuration;
using SFA.DAS.NotificationService.Application;
using SFA.DAS.NotificationService.Application.Interfaces;
using SFA.DAS.NotificationService.Application.Messages;

namespace SFA.DAS.NotificationService.Infrastructure
{
    public class NotifyEmailService : IEmailService
    {
        private readonly IConfigurationService _configurationService;

        public NotifyEmailService(IConfigurationService configurationService)
        {
            if (configurationService == null)
                throw new ArgumentNullException(nameof(configurationService));
            _configurationService = configurationService;
        }

        public async Task SendAsync(EmailMessage message)
        {
            var notifyMessage = new NotifyMessage
            {
                To = message.RecipientsAddress,
                Template = message.TemplateId,
                Personalisation = message.Data.ToDictionary(item => item.Key.ToLower(), item => item.Value)
            };

            await SendMessage(notifyMessage);
        }

        public async Task SendMessage(NotifyMessage content)
        {
            using (var httpClient = await CreateHttpClient())
            {
                var serializeObject = JsonConvert.SerializeObject(content);
                var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Post, "/notifications/email")
                {
                    Content = new StringContent(serializeObject, Encoding.UTF8, "application/json")
                });
                response.EnsureSuccessStatusCode();
            }
        }

        private async Task<HttpClient> CreateHttpClient()
        {
            var configuration = await _configurationService.GetAsync<NotificationServiceConfiguration>();
            return new HttpClient
            {
                BaseAddress = new Uri(configuration.NotifyEmail.ApiBaseUrl)
            };
        }
    }
    
    public class NotifyMessage
    {
        [JsonProperty(PropertyName = "to")]
        public string To { get; set; }
        [JsonProperty(PropertyName = "template")]
        public string Template { get; set; }
        [JsonProperty(PropertyName = "personalisation")]
        public Dictionary<string, string> Personalisation { get; set; }
    }

}