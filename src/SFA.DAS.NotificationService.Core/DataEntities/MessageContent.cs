using System;
using SFA.DAS.NotificationService.Application.Messages;

namespace SFA.DAS.NotificationService.Application.DataEntities
{
    public class MessageContent
    {
        public string UserId { get; set; }
        public DateTime Timestamp { get; set; }
        public MessageFormat MessageFormat { get; set; }
        public bool ForceFormat { get; set; }
        public string TemplateId { get; set; }
        public string Data { get; set; }    
    }
}