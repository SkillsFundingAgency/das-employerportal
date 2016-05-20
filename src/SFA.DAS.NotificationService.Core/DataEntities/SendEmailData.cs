using System;
using System.Collections.Generic;
using SFA.DAS.NotificationService.Application.Messages;

namespace SFA.DAS.NotificationService.Application.DataEntities
{
    public class SendEmailData
    {
        public string UserId { get; set; }
        public List<KeyValuePair<string, string>> Data { get; set; }
        public DateTime Timestamp { get; set; }
        public MessageStatus Status { get; set; }
    }
}