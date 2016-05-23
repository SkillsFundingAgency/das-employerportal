using FluentValidation;

namespace SFA.DAS.NotificationService.Application.Commands.SendMessage
{
    public class SendMessageCommandValidator : AbstractValidator<SendMessageCommand>
    {
        public SendMessageCommandValidator()
        {
            //RuleFor(model => model.UserId).NotEmpty();
            //RuleFor(model => model.FromEmail).NotEmpty();
            //RuleFor(model => model.ToEmail).NotEmpty();
            //RuleFor(model => model.Subject).NotEmpty();
            //RuleFor(model => model.Message).NotEmpty();
        }
    }
}