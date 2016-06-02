using System.Collections.Generic;
using SFA.DAS.NotificationService.Api.Core;

namespace SFA.DAS.NotificationService.Api.Orchestrators
{
    public interface INotificationOrchestrator
    {
        OrchestratorResponse Post(Dictionary<string, string> notification);
    }
}