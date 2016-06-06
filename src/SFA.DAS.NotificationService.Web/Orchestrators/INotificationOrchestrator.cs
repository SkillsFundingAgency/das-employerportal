using System.Threading.Tasks;
using SFA.DAS.NotificationService.Api.Core;
using SFA.DAS.NotificationService.Api.Models;

namespace SFA.DAS.NotificationService.Api.Orchestrators
{
    public interface INotificationOrchestrator
    {
        Task<OrchestratorResponse> SendEmail(EmailViewModel notification);
    }
}