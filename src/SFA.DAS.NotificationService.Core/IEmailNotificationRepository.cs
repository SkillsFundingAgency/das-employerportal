using System.Threading.Tasks;
using SFA.DAS.NotificationService.Core.Messages;

namespace SFA.DAS.NotificationService.Core
{
    public interface IEmailNotificationRepository
    {
        Task<string> CreateAsync(SendEmailMessage message);
        Task<SendEmailMessage> GetAsync(string userId, string messageId);
    }
}