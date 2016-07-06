using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SFA.DAS.Configuration;
using SFA.DAS.NotificationService.Application;
using SFA.DAS.NotificationService.Application.Services;

namespace SFA.DAS.NotificationService.Infrastructure.Notify
{
    public interface INotifyHttpClientWrapper
    {
        Task<HttpResponseMessage> SendMessage(NotifyMessage content);
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

        public async Task<HttpResponseMessage> SendMessage(NotifyMessage content)
        {
            var configuration = await _configurationService.GetAsync<NotificationServiceConfiguration>();

            //TODO: Remove this line when we have proper email templates
            content.Template = configuration.NotifyEmail.EmailTemplateId;

            using (var httpClient = CreateHttpClient(configuration.NotifyEmail.ApiBaseUrl))
            {
                var token = JwtTokenCreation.CreateToken(configuration.NotifyEmail.ServiceId, configuration.NotifyEmail.ApiKey);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var serializeObject = JsonConvert.SerializeObject(content);
                var stringContent = new StringContent(serializeObject, Encoding.UTF8, "application/json");

                return await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Post, "/notifications/email")
                {
                    Content = stringContent
                });
            }
        }

        private HttpClient CreateHttpClient(string baseUrl)
        {
            return new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };
        }
    }
}