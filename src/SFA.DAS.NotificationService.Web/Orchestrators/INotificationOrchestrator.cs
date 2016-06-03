using SFA.DAS.NotificationService.Api.Core;
using SFA.DAS.NotificationService.Api.Models;

namespace SFA.DAS.NotificationService.Api.Orchestrators
{
    public interface INotificationOrchestrator
    {
        OrchestratorResponse SendEmail(EmailViewModel notification);
    }
}