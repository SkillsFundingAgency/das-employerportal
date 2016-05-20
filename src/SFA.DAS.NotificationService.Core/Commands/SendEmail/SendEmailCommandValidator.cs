using FluentValidation;

namespace SFA.DAS.NotificationService.Application.Commands.SendEmail
{
    public class SendEmailCommandValidator : AbstractValidator<SendEmailCommand>
    {
        public SendEmailCommandValidator()
        {
            RuleFor(model => model.UserId).NotEmpty();
            //RuleFor(model => model.FromEmail).NotEmpty();
            //RuleFor(model => model.ToEmail).NotEmpty();
            //RuleFor(model => model.Subject).NotEmpty();
            //RuleFor(model => model.Message).NotEmpty();
        }
    }
}