using System.Threading.Tasks;
using SFA.DAS.NotificationService.Application.Messages;

namespace SFA.DAS.NotificationService.Application.Interfaces
{
    public interface ISmsService
    {
        Task SendAsync(SmsMessage message);
    }
}