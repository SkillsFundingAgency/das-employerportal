using System;
using FluentValidation.Results;
using MediatR;
using SFA.DAS.Messaging;
using SFA.DAS.NotificationService.Application.DataEntities;
using SFA.DAS.NotificationService.Application.Exceptions;
using SFA.DAS.NotificationService.Application.Interfaces;
using SFA.DAS.NotificationService.Application.Messages;
using SFA.DAS.TimeProvider;

namespace SFA.DAS.NotificationService.Application.Commands.SendMessage
{
    public class SendMessageCommandHandler : RequestHandler<SendMessageCommand>
    {
        private readonly IMessageNotificationRepository _emailNotificationRepository;
        private readonly MessagingService _messagingService;

        public SendMessageCommandHandler(IMessageNotificationRepository emailNotificationRepository, MessagingService messagingService)
        {
            if (emailNotificationRepository == null)
                throw new ArgumentNullException(nameof(emailNotificationRepository));
            if (messagingService == null)
                throw new ArgumentNullException(nameof(messagingService));
            _emailNotificationRepository = emailNotificationRepository;
            _messagingService = messagingService;
        }

        protected override void HandleCore(SendMessageCommand message)
        {
            var validationResult = Validate(message);

            if (!validationResult.IsValid)
                throw new CustomValidationException(validationResult);

            var messageType = GetMessageType(message);
            var messageId = Guid.NewGuid().ToString();
            message.Data.Add("Timestamp", DateTimeProvider.Current.UtcNow.ToString("yyyy-MM-dd HH':'mm':'ss"));

            _emailNotificationRepository.Create(new MessageData
            {
                MessageId = messageId,
                MessageType = messageType,
                Data = message.Data
            });

            _messagingService.PublishAsync(new QueueMessage
            {
                MessageType = messageType,
                MessageId = messageId
            }).Wait();
        }

        private string GetMessageType(SendMessageCommand message)
        {
            var messageType = "";

            message.Data.TryGetValue("messagetype", out messageType);

            return messageType;
        }

        private ValidationResult Validate(SendMessageCommand cmd)
        {
            var validator = new SendMessageCommandValidator();

            return validator.Validate(cmd);
        }

    }
}