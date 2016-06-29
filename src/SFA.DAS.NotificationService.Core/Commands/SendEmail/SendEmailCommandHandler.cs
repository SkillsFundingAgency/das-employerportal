using System;
using System.Threading.Tasks;
using FluentValidation.Results;
using MediatR;
using Newtonsoft.Json;
using NLog;
using SFA.DAS.Messaging;
using SFA.DAS.NotificationService.Application.DataEntities;
using SFA.DAS.NotificationService.Application.Exceptions;
using SFA.DAS.NotificationService.Application.Interfaces;
using SFA.DAS.NotificationService.Application.Messages;
using SFA.DAS.NotificationService.Application.Services;
using SFA.DAS.TimeProvider;

namespace SFA.DAS.NotificationService.Application.Commands.SendEmail
{
    public class SendEmailCommandHandler : AsyncRequestHandler<SendEmailCommand>
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

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

        protected override async Task HandleCore(SendEmailCommand message)
        {
            var messageId = GuidProvider.Current.NewGuid().ToString();

            Logger.Debug($"Received SendEmailCommand for message type {message.MessageType} to send to {message.RecipientsAddress} (message id: {messageId})");

            var validationResult = Validate(message);

            if (!validationResult.IsValid)
                throw new CustomValidationException(validationResult);


            await _emailNotificationRepository.Create(CreateMessageData(message, messageId));

            Logger.Debug($"Stored message '{messageId}' in data store");

            await _messagingService.PublishAsync(new QueueMessage
            {
                MessageType = message.MessageType,
                MessageId = messageId
            });
            Logger.Debug($"Published message '{messageId}' to queue");
        }

        private static MessageData CreateMessageData(SendEmailCommand message, string messageId)
        {
            return new MessageData
            {
                MessageId = messageId,
                MessageType = message.MessageType,
                Content = new MessageContent
                {
                    UserId = message.UserId,
                    Timestamp = DateTimeProvider.Current.UtcNow,
                    MessageFormat = MessageFormat.Email,
                    ForceFormat = message.ForceFormat,
                    TemplateId = message.TemplateId,
                    Data = JsonConvert.SerializeObject(new EmailContent
                    {
                        RecipientsAddress = message.RecipientsAddress,
                        ReplyToAddress = message.ReplyToAddress,
                        Data = message.Data
                    })
                }
            };
        }

        private ValidationResult Validate(SendEmailCommand cmd)
        {
            var validator = new SendEmailCommandValidator();

            return validator.Validate(cmd);
        }

    }
}