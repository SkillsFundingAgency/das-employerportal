using System;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.NotificationService.Application.Interfaces;
using SFA.DAS.NotificationService.Application.Messages;

namespace SFA.DAS.NotificationService.Infrastructure.Notify
{
    public class NotifySmsService : ISmsService
    {
        private readonly INotifyHttpClientWrapper _httpClientWrapper;

        public NotifySmsService(INotifyHttpClientWrapper httpClientWrapper)
        {
            if (httpClientWrapper == null)
                throw new ArgumentNullException(nameof(httpClientWrapper));
            _httpClientWrapper = httpClientWrapper;
        }

        public async Task SendAsync(SmsMessage message)
        {
            var notifyMessage = new NotifyMessage
            {
                To = message.SendTo,
                Template = message.TemplateId,
                Personalisation = message.Data.ToDictionary(item => item.Key.ToLower(), item => item.Value)
            };

            await _httpClientWrapper.SendMessage(notifyMessage);
        }
    }
}