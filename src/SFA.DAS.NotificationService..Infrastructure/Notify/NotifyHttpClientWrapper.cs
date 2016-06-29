using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SFA.DAS.Configuration;
using SFA.DAS.NotificationService.Application;

namespace SFA.DAS.NotificationService.Infrastructure.Notify
{
    public interface INotifyHttpClientWrapper
    {
        Task SendMessage(NotifyMessage content);
    }

    public class NotifyHttpClientWrapper : INotifyHttpClientWrapper
    {
        private readonly IConfigurationService _configurationService;

        public NotifyHttpClientWrapper(IConfigurationService configurationService)
        {
            if (configurationService == null)
                throw new ArgumentNullException(nameof(configurationService));
            _configurationService = configurationService;
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
}