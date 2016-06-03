using System.Collections.Generic;

namespace SFA.DAS.NotificationService.Application.Messages
{
    public class EmailMessage
    {
        public string UserId { get; set; }
        public string MessageType { get; set; }
        public string RecipientsAddress { get; set; }
        public string ReplyToAddress { get; set; }
        public Dictionary<string, string> Data { get; set; }
    }
}