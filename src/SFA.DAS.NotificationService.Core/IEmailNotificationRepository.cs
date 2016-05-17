using System.Threading.Tasks;
using SFA.DAS.NotificationService.Core.Messages;

namespace SFA.DAS.NotificationService.Core
{
    public interface IEmailNotificationRepository
    {
        Task<string> Create(SendEmailMessage message);
        Task<SendEmailMessage> Get(string userId, string messageId);
    }
}