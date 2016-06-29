using System.Threading.Tasks;
using SFA.DAS.NotificationService.Application.Interfaces;
using SFA.DAS.NotificationService.Application.Messages;

namespace SFA.DAS.NotificationService.Infrastructure.Notify
{
    public class NotifySmsService : ISmsService
    {
        public Task SendAsync(SmsMessage message)
        {
            throw new System.NotImplementedException();
        }
    }
}