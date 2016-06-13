using SFA.DAS.NotificationService.Application.DataEntities;

namespace SFA.DAS.NotificationService.Application.Interfaces
{
    public interface IMessageNotificationRepository
    {
        void Create(MessageData message);
        MessageData Get(string messageType, string messageId);
    }
}