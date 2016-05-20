using System;
using FluentValidation.Results;
using MediatR;
using SFA.DAS.NotificationService.Application.DataEntities;
using SFA.DAS.NotificationService.Application.Exceptions;
using SFA.DAS.NotificationService.Application.Interfaces;
using SFA.DAS.NotificationService.Application.Messages;
using SFA.DAS.TimeProvider;

namespace SFA.DAS.NotificationService.Application.Commands.SendEmail
{
    public class SendEmailCommandHandler : RequestHandler<SendEmailCommand>
    {
        private readonly IEmailNotificationRepository _emailNotificationRepository;

        public SendEmailCommandHandler(IEmailNotificationRepository emailNotificationRepository)
        {
            if (emailNotificationRepository == null)
                throw new ArgumentNullException(nameof(emailNotificationRepository));
            _emailNotificationRepository = emailNotificationRepository;
        }

        protected override void HandleCore(SendEmailCommand message)
        {
            var validationResult = Validate(message);

            if (!validationResult.IsValid)
                throw new CustomValidationException(validationResult);

            _emailNotificationRepository.Create(new SendEmailData
            {
                UserId = message.UserId,
                ToEmail = message.ToEmail,
                FromEmail = message.FromEmail,
                Subject = message.Subject,
                Message = message.Message,
                Timestamp = DateTimeProvider.Current.UtcNow,
                Status = MessageStatus.Received
            });
        }

        private ValidationResult Validate(SendEmailCommand cmd)
        {
            var validator = new SendEmailCommandValidator();

            return validator.Validate(cmd);
        }

    }
}