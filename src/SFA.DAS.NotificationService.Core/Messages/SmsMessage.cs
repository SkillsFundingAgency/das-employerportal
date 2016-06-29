using System.Collections.Generic;

namespace SFA.DAS.NotificationService.Application.Messages
{
    public class SmsMessage
    {
        public string UserId { get; set; }
        public string MessageType { get; set; }
        public string TemplateId { get; set; }
        public string SendTo { get; set; }
        public string ReplyTo { get; set; }
        public Dictionary<string, string> Data { get; set; }

    }
}