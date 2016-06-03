using System;
using System.Collections.Generic;
using MediatR;
using SFA.DAS.NotificationService.Api.Core;
using SFA.DAS.NotificationService.Api.Models;
using SFA.DAS.NotificationService.Application.Commands.SendEmail;

namespace SFA.DAS.NotificationService.Api.Orchestrators
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

        public OrchestratorResponse SendEmail(EmailViewModel notification)
        {
            _mediator.Send(new SendEmailCommand
            {
                UserId = notification.UserId,
                MessageType = notification.MessageType,
                ForceFormat = notification.ForceFormat,
                RecipientsAddress = notification.RecipientsAddress,
                ReplyToAddress = notification.ReplyToAddress,
                Data = notification.Data
            });
            
            return GetOrchestratorResponse(NotificationOrchestratorCodes.Post.Success);
        }
    }
}