using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NLog;
using Polly;
using Polly.CircuitBreaker;
using SFA.DAS.NotificationService.Application.Interfaces;
using SFA.DAS.NotificationService.Application.Messages;

namespace SFA.DAS.NotificationService.Infrastructure.Notify
{
    public class NotifyEmailService : IEmailService
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private readonly INotifyHttpClientWrapper _clientWrapper;
        private static readonly CircuitBreakerPolicy<HttpStatusCode> CircuitBreakerPolicy;

        public NotifyEmailService(INotifyHttpClientWrapper clientWrapper)
        {
            if (clientWrapper == null)
                throw new ArgumentNullException(nameof(clientWrapper));
            _clientWrapper = clientWrapper;
        }

        static NotifyEmailService()
        {
            CircuitBreakerPolicy = Policy
                .HandleResult(HttpStatusCode.Unauthorized)
                .OrResult(HttpStatusCode.Forbidden)
                .OrResult((HttpStatusCode)429)
                .CircuitBreakerAsync(1, TimeSpan.FromHours(1));
        }

        public HttpStatusCode StatusCode { get; private set; }

        public async Task SendAsync(EmailMessage message)
        {
            Logger.Info($"Sending {message.MessageType} to {message.RecipientsAddress}");

            var notifyMessage = new NotifyMessage
            {
                To = message.RecipientsAddress,
                Template = message.TemplateId,
                Personalisation = message.Data.ToDictionary(item => item.Key.ToLower(), item => item.Value)
            };

            if (CircuitBreakerPolicy.CircuitState == CircuitState.Closed)
            {
                StatusCode = await CircuitBreakerPolicy.ExecuteAsync(() => Send(notifyMessage));
                return;
            }

            StatusCode = HttpStatusCode.ServiceUnavailable;
        }

        public void CloseCircuitBreaker()
        {
            CircuitBreakerPolicy.Reset();
        }

        private async Task<HttpStatusCode> Send(NotifyMessage message)
        {
            var response = await _clientWrapper.SendMessage(message);

            Logger.Info($"Notify Service returned {response.StatusCode}");

            if (response.StatusCode == HttpStatusCode.Created)
            {
                var content = await response.Content.ReadAsStringAsync();

                var notifyResponse = JsonConvert.DeserializeObject<NotifyResponse>(content);

                var messageId = notifyResponse.Data.Notification.Id;

                //TODO: Store messageId against original message?
                //Needed to query Notify to determine state of message
                //As we have links to resend in the UI, do we need to verify sending? 
            }

            return response.StatusCode;
        }
    }
}