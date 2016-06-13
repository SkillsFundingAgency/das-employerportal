using FluentValidation;

namespace SFA.DAS.NotificationService.Application.Commands.SendEmail
{
    public class SendEmailCommandValidator : AbstractValidator<SendEmailCommand>
    {
        public SendEmailCommandValidator()
        {
            RuleFor(model => model.UserId).NotEmpty();
            RuleFor(model => model.MessageType).NotEmpty();
            RuleFor(model => model.RecipientsAddress).NotEmpty();
            RuleFor(model => model.ReplyToAddress).NotEmpty();
        }
    }
}