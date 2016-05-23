using System;
using FluentValidation.Results;
using MediatR;
using SFA.DAS.NotificationService.Application.DataEntities;
using SFA.DAS.NotificationService.Application.Exceptions;
using SFA.DAS.NotificationService.Application.Interfaces;

namespace SFA.DAS.NotificationService.Application.Commands.SendMessage
{
    public class SendMessageCommandHandler : RequestHandler<SendMessageCommand>
    {
        private readonly IMessageNotificationRepository _emailNotificationRepository;

        public SendMessageCommandHandler(IMessageNotificationRepository emailNotificationRepository)
        {
            if (emailNotificationRepository == null)
                throw new ArgumentNullException(nameof(emailNotificationRepository));
            _emailNotificationRepository = emailNotificationRepository;
        }

        protected override void HandleCore(SendMessageCommand message)
        {
            var validationResult = Validate(message);

            if (!validationResult.IsValid)
                throw new CustomValidationException(validationResult);

            var messageType = GetMessageType(message);

            _emailNotificationRepository.Create(new MessageData
            {
                MessageId = Guid.NewGuid().ToString(),
                MessageType = messageType,
                Data = message.Data
            });
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