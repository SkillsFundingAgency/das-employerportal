using SFA.DAS.NotificationService.Application.DataEntities;
using SFA.DAS.NotificationService.Application.Messages;

namespace SFA.DAS.NotificationService.Application.Interfaces
{
    public interface IEmailNotificationRepository
    {
        string Create(SendEmailData message);
        SendEmailMessage Get(string userId, string messageId);
    }
}