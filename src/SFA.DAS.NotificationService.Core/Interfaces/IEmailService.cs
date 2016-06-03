using SFA.DAS.NotificationService.Application.Messages;

namespace SFA.DAS.NotificationService.Application.Interfaces
{
    public interface IEmailService
    {
        void Send(EmailMessage message);
    }
}