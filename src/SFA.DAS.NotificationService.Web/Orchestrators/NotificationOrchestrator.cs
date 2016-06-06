using System;
using System.Collections.Generic;
using MediatR;
using NLog;
using SFA.DAS.NotificationService.Api.Core;
using SFA.DAS.NotificationService.Api.Models;
using SFA.DAS.NotificationService.Application.Commands.SendEmail;

namespace SFA.DAS.NotificationService.Api.Orchestrators
{
    public class NotificationOrchestrator : OrchestratorBase, INotificationOrchestrator
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        private readonly IMediator _mediator;

        public NotificationOrchestrator(IMediator mediator)
        {
            if (mediator == null)
                throw new ArgumentNullException(nameof(mediator));
            _mediator = mediator;
        }

        public OrchestratorResponse SendEmail(EmailViewModel notification)
        {
            try
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
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                throw;
            }
        }
    }
}