using System.Threading.Tasks;
using SFA.DAS.NotificationService.Application.DataEntities;

namespace SFA.DAS.NotificationService.Application.Interfaces
{
    public interface IMessageNotificationRepository
    {
        Task Create(MessageData message);
        Task<MessageData> Get(string messageType, string messageId);
    }
}