using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Messaging;
using SFA.DAS.NotificationService.Application;
using SFA.DAS.NotificationService.Application.Commands.SendEmail;
using SFA.DAS.NotificationService.Application.Interfaces;
using SFA.DAS.NotificationService.Application.Messages;
using SFA.DAS.NotificationService.Web.Core;
using SFA.DAS.NotificationService.Web.Models;
using SFA.DAS.TimeProvider;

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

        public OrchestratorResponse Post(EmailNotification notification)
        {
            _mediator.Send(new SendEmailCommand
            {
                UserId = notification.UserId,
                Data = notification.Data
            });
            
            return GetOrchestratorResponse(NotificationOrchestratorCodes.Post.Success);
        }
    }
}