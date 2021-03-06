﻿using System;
using System.Threading.Tasks;
using FluentValidation.Results;
using MediatR;
using NLog;
using SFA.DAS.NotificationService.Api.Core;
using SFA.DAS.NotificationService.Api.Models;
using SFA.DAS.NotificationService.Application.Commands.SendEmail;
using SFA.DAS.NotificationService.Application.Exceptions;

namespace SFA.DAS.NotificationService.Api.Orchestrators
{
    public class NotificationOrchestrator : OrchestratorBase, INotificationOrchestrator
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private readonly IMediator _mediator;

        public NotificationOrchestrator(IMediator mediator)
        {
            if (mediator == null)
                throw new ArgumentNullException(nameof(mediator));
            _mediator = mediator;
        }

        public async Task<OrchestratorResponse> SendEmail(EmailViewModel notification)
        {
            try
            {
                var cmd = new SendEmailCommand
                {
                    UserId = notification.UserId,
                    MessageType = notification.MessageType,
                    ForceFormat = notification.ForceFormat,
                    RecipientsAddress = notification.RecipientsAddress,
                    ReplyToAddress = notification.ReplyToAddress,
                    Data = notification.Data
                };

                var validationResult = ValidateCommand(cmd);

                if (!validationResult.IsValid)
                    return GetOrchestratorResponse(NotificationOrchestratorCodes.Post.ValidationFailure, validationResult: validationResult);

                await _mediator.SendAsync(cmd);

                return GetOrchestratorResponse(NotificationOrchestratorCodes.Post.Success);
            }
            catch (CustomValidationException ex)
            {
                Logger.Info($"Validation error {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, ex.Message);
                throw;
            }
        }

        private static ValidationResult ValidateCommand(SendEmailCommand cmd)
        {
            var validator = new SendEmailCommandValidator();

            return validator.Validate(cmd);
        }
    }
}