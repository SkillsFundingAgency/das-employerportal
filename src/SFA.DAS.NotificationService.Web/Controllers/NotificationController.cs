using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using SFA.DAS.NotificationService.Web.Orchestrators;

namespace SFA.DAS.NotificationService.Web.Controllers
{
    public class NotificationController : ApiController
    {
        private readonly INotificationOrchestrator _orchestrator;

        public NotificationController(INotificationOrchestrator orchestrator)
        {
            if (orchestrator == null)
                throw new ArgumentNullException(nameof(orchestrator));
            _orchestrator = orchestrator;
        }

        public async Task<HttpResponseMessage> Post(Dictionary<string, string> notification)
        {
            return await Task.Run<HttpResponseMessage>(() =>
            {
                var response = _orchestrator.Post(notification);

                return new HttpResponseMessage(HttpStatusCode.OK);
            });
        }
    }
}
