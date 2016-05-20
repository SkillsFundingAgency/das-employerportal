using System;
using MediatR;
using SFA.DAS.NotificationService.Application.Messages;

namespace SFA.DAS.NotificationService.Application.Commands.SendEmail
{
    public class SendEmailCommand : IRequest
    {
        public string UserId { get; set; }
        public string ToEmail { get; set; }
        public string FromEmail { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}