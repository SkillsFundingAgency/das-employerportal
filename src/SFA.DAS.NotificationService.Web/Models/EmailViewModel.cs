using System.Collections.Generic;

namespace SFA.DAS.NotificationService.Api.Models
{
    public class EmailViewModel
    {
        public string UserId { get; set; }
        public string MessageType { get; set; }
        public string RecipientsAddress { get; set; }
        public string ReplyToAddress { get; set; }
        public bool ForceFormat { get; set; }
        public Dictionary<string, string> Data { get; set; }
    }
}