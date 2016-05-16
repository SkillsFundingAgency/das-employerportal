using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SFA.DAS.NotificationService.Core.Interfaces;
using SFA.DAS.NotificationService.Core.Messages;
using SFA.DAS.NotificationService.Web.Models;

namespace SFA.DAS.NotificationService.Web.Controllers
{
    public class NotificationController : ApiController
    {
        private readonly IQueueWriter _queueWriter;

        public NotificationController(IQueueWriter queueWriter)
        {
            if (queueWriter == null)
                throw new ArgumentNullException(nameof(queueWriter));

            _queueWriter = queueWriter;
        }

        public HttpResponseMessage Post(EmailNotification notification)
        {
            _queueWriter.Write(MapFrom(notification));

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        private SendEmailMessage MapFrom(EmailNotification notification)
        {
            return new SendEmailMessage
            {
                FromEmail = notification.FromEmail,
                ToEmail = notification.ToEmail,
                Subject = notification.Subject,
                Message = notification.Message
            };
        }
    }
}
