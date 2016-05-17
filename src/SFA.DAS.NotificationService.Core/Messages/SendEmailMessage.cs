using System;

namespace SFA.DAS.NotificationService.Core.Messages
{
    public class SendEmailMessage
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