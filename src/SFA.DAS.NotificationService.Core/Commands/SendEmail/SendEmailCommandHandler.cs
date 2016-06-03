using System;
using FluentValidation.Results;
using MediatR;
using Newtonsoft.Json;
using SFA.DAS.Messaging;
using SFA.DAS.NotificationService.Application.DataEntities;
using SFA.DAS.NotificationService.Application.Exceptions;
using SFA.DAS.NotificationService.Application.Interfaces;
using SFA.DAS.NotificationService.Application.Messages;
using SFA.DAS.NotificationService.Application.Services;
using SFA.DAS.TimeProvider;

namespace SFA.DAS.NotificationService.Application.Commands.SendEmail
{
    public class SendEmailCommandHandler : RequestHandler<SendEmailCommand>
    {
        private readonly IMessageNotificationRepository _emailNotificationRepository;
        private readonly MessagingService _messagingService;

        public SendEmailCommandHandler(IMessageNotificationRepository emailNotificationRepository, MessagingService messagingService)
        {
            if (emailNotificationRepository == null)
                throw new ArgumentNullException(nameof(emailNotificationRepository));
            if (messagingService == null)
                throw new ArgumentNullException(nameof(messagingService));
            _emailNotificationRepository = emailNotificationRepository;
            _messagingService = messagingService;
        }

        protected override void HandleCore(SendEmailCommand message)
        {
            var validationResult = Validate(message);

            if (!validationResult.IsValid)
                throw new CustomValidationException(validationResult);

            var messageId = GuidProvider.Current.NewGuid().ToString();

            _emailNotificationRepository.Create(new MessageData
            {
                MessageId = messageId,
                MessageType = message.MessageType,
                Content = new MessageContent
                {
                    UserId = message.UserId,
                    Timestamp = DateTimeProvider.Current.UtcNow,
                    MessageFormat = MessageFormat.Email,
                    ForceFormat = message.ForceFormat,
                    Data = JsonConvert.SerializeObject(new EmailContent
                    {
                        RecipientsAddress = message.RecipientsAddress,
                        ReplyToAddress = message.ReplyToAddress,
                        Data = message.Data
                    })
                }
            });

            _messagingService.PublishAsync(new QueueMessage
            {
                MessageType = message.MessageType,
                MessageId = messageId
            }).Wait();
        }

        private ValidationResult Validate(SendEmailCommand cmd)
        {
            var validator = new SendEmailCommandValidator();

            return validator.Validate(cmd);
        }

    }
}