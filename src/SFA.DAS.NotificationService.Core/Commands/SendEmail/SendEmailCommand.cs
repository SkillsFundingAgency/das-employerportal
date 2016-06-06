using System.Collections.Generic;
using MediatR;

namespace SFA.DAS.NotificationService.Application.Commands.SendEmail
{
    public class SendEmailCommand : IAsyncRequest
    {
        public string UserId { get; set; }
        public string MessageType { get; set; }
        public string RecipientsAddress { get; set; }
        public string ReplyToAddress { get; set; }
        public bool ForceFormat { get; set; }
        public Dictionary<string, string> Data { get; set; }
    }
}