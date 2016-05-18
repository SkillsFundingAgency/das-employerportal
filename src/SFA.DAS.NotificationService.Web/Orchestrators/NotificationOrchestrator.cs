using System;
using System.Threading.Tasks;
using SFA.DAS.Messaging;
using SFA.DAS.NotificationService.Core;
using SFA.DAS.NotificationService.Core.Messages;
using SFA.DAS.NotificationService.Web.Core;
using SFA.DAS.NotificationService.Web.Models;
using SFA.DAS.TimeProvider;

namespace SFA.DAS.NotificationService.Web.Orchestrators
{
    public class NotificationOrchestrator : OrchestratorBase, INotificationOrchestrator
    {
        private readonly MessagingService _messagingService;
        private readonly IEmailNotificationRepository _emailNotificationRepository;

        public NotificationOrchestrator(MessagingService messagingService, IEmailNotificationRepository emailNotificationRepository)
        {
            if (messagingService == null)
                throw new ArgumentNullException(nameof(messagingService));
            if (emailNotificationRepository == null)
                throw new ArgumentNullException(nameof(emailNotificationRepository));
            _messagingService = messagingService;
            _emailNotificationRepository = emailNotificationRepository;
        }

        public async Task<OrchestratorResponse> Post(EmailNotification notification)
        {
            var message = new SendEmailMessage
            {
                UserId = notification.UserId,
                FromEmail = notification.FromEmail,
                ToEmail = notification.ToEmail,
                Subject = notification.Subject,
                Message = notification.Message,
                Timestamp = DateTimeProvider.Current.UtcNow,
                Status = MessageStatus.Received
            };

            await _emailNotificationRepository.CreateAsync(message);

            await _messagingService.PublishAsync(message);


            return GetOrchestratorResponse(NotificationOrchestratorCodes.Post.Success);
        }
    }
}