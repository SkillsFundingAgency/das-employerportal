using SFA.DAS.NotificationService.Core.Messages;

namespace SFA.DAS.NotificationService.Core.Interfaces
{
    public interface IQueueReader
    {
        SendEmailMessage Read();
    }
}