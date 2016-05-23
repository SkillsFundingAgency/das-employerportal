using System.Collections.Generic;
using MediatR;

namespace SFA.DAS.NotificationService.Application.Commands.SendMessage
{
    public class SendMessageCommand : IRequest
    {
        public Dictionary<string, string> Data { get; set; }
    }
}