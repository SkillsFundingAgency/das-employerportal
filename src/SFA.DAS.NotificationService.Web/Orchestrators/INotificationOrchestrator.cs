using SFA.DAS.NotificationService.Web.Core;
using SFA.DAS.NotificationService.Web.Models;

namespace SFA.DAS.NotificationService.Web.Orchestrators
{
    public interface INotificationOrchestrator
    {
        OrchestratorResponse Post(EmailNotification notification);
    }
}