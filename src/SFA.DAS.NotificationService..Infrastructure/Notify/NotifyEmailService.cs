using System;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.NotificationService.Application.Interfaces;
using SFA.DAS.NotificationService.Application.Messages;

namespace SFA.DAS.NotificationService.Infrastructure.Notify
{
    public class NotifyEmailService : IEmailService
    {
        private readonly INotifyHttpClientWrapper _clientWrapper;

        public NotifyEmailService(INotifyHttpClientWrapper clientWrapper)
        {
            if (clientWrapper == null)
                throw new ArgumentNullException(nameof(clientWrapper));
            _clientWrapper = clientWrapper;
        }

        public async Task SendAsync(EmailMessage message)
        {
            var notifyMessage = new NotifyMessage
            {
                To = message.RecipientsAddress,
                Template = message.TemplateId,
                Personalisation = message.Data.ToDictionary(item => item.Key.ToLower(), item => item.Value)
            };

            await _clientWrapper.SendMessage(notifyMessage);
        }
    }
}