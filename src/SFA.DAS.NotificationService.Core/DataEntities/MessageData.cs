using System.Collections.Generic;

namespace SFA.DAS.NotificationService.Application.DataEntities
{
    public class MessageData
    {
        public string MessageType { get; set; }
        public string MessageId { get; set; }
        public Dictionary<string, string> Data { get; set; }
    }
}