using System;
using SFA.DAS.NotificationService.Application.Messages;

namespace SFA.DAS.NotificationService.Application.DataEntities
{
    public class SendEmailData
    {
        public string UserId { get; set; }
        public string ToEmail { get; set; }
        public string FromEmail { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
        public MessageStatus Status { get; set; }
    }
}