using System.Threading.Tasks;
using SFA.DAS.NotificationService.Application.Messages;

namespace SFA.DAS.NotificationService.Application
{
    public interface IEmailNotificationRepository
    {
        Task<string> CreateAsync(SendEmailMessage message);
        Task<SendEmailMessage> GetAsync(string userId, string messageId);
    }
}