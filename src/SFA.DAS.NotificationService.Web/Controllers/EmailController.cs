using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using SFA.DAS.NotificationService.Api.Models;
using SFA.DAS.NotificationService.Api.Orchestrators;

namespace SFA.DAS.NotificationService.Api.Controllers
{
    public class EmailController : ApiController
    {
        private readonly INotificationOrchestrator _orchestrator;

        public EmailController(INotificationOrchestrator orchestrator)
        {
            if (orchestrator == null)
                throw new ArgumentNullException(nameof(orchestrator));
            _orchestrator = orchestrator;
        }

        public async Task<HttpResponseMessage> Post(EmailViewModel notification)
        {

            await _orchestrator.SendEmail(notification);

            return new HttpResponseMessage(HttpStatusCode.OK);

        }
    }
}