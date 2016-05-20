using System;
using System.Collections.Generic;
using MediatR;
using SFA.DAS.NotificationService.Application.Messages;

namespace SFA.DAS.NotificationService.Application.Commands.SendEmail
{
    public class SendEmailCommand : IRequest
    {
        public string UserId { get; set; }
        public List<KeyValuePair<string, string>> Data { get; set; }
    }
}