using System.Collections.Generic;

namespace SFA.DAS.NotificationService.Application.Messages
{
    public class EmailContent
    {
        public string RecipientsAddress { get; set; }
        public string ReplyToAddress { get; set; }
        public Dictionary<string, string> Data { get; set; }
    }
}