using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using SFA.DAS.Messaging;
using SFA.DAS.NotificationService.Core;
using SFA.DAS.NotificationService.Core.Messages;
using SFA.DAS.NotificationService.Web.Models;

namespace SFA.DAS.NotificationService.Web.Controllers
{
    public class NotificationController : ApiController
    {
        private readonly MessagingService _messagingService;
        private readonly IEmailNotificationRepository _emailNotificationRepository;

        public NotificationController(MessagingService messagingService, IEmailNotificationRepository emailNotificationRepository)
        {
            _messagingService = messagingService;
            _emailNotificationRepository = emailNotificationRepository;
        }

        public async Task<HttpResponseMessage> Post(EmailNotification notification)
        {
            var message = new SendEmailMessage
            {
                UserId = notification.UserId,
                FromEmail = notification.FromEmail,
                ToEmail = notification.ToEmail,
                Subject = notification.Subject,
                Message = notification.Message,
                Timestamp = DateTime.Now,
                Status = MessageStatus.Received
            };

            await _emailNotificationRepository.CreateAsync(message);

            await _messagingService.PublishAsync(message);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
