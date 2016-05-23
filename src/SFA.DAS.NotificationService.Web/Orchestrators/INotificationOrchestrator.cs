using System.Collections.Generic;
using SFA.DAS.NotificationService.Web.Core;

namespace SFA.DAS.NotificationService.Web.Orchestrators
{
    public interface INotificationOrchestrator
    {
        OrchestratorResponse Post(Dictionary<string, string> notification);
    }
}