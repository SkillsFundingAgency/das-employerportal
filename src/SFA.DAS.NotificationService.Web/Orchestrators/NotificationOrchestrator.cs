using System;
using System.Collections.Generic;
using MediatR;
using SFA.DAS.NotificationService.Application.Commands.SendMessage;
using SFA.DAS.NotificationService.Web.Core;

namespace SFA.DAS.NotificationService.Web.Orchestrators
{
    public class NotificationOrchestrator : OrchestratorBase, INotificationOrchestrator
    {
        private readonly IMediator _mediator;

        public NotificationOrchestrator(IMediator mediator)
        {
            if (mediator == null)
                throw new ArgumentNullException(nameof(mediator));
            _mediator = mediator;
        }

        public OrchestratorResponse Post(Dictionary<string, string> notification)
        {
            _mediator.Send(new SendMessageCommand
            {
                Data = notification
            });
            
            return GetOrchestratorResponse(NotificationOrchestratorCodes.Post.Success);
        }
    }
}