using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using SFA.DAS.NotificationService.Api.Models;
using SFA.DAS.NotificationService.Api.Orchestrators;

namespace SFA.DAS.NotificationService.Api.Controllers
{
    [Authorize]
    [RoutePrefix("api/email")]
    public class EmailController : ApiController
    {
        private readonly INotificationOrchestrator _orchestrator;

        public EmailController(INotificationOrchestrator orchestrator)
        {
            if (orchestrator == null)
                throw new ArgumentNullException(nameof(orchestrator));
            _orchestrator = orchestrator;
        }

        [Route("")]
        [HttpPost]
        public async Task<HttpResponseMessage> Post(EmailViewModel notification)
        {

            var response = await _orchestrator.SendEmail(notification);

            if (response.Code == NotificationOrchestratorCodes.Post.ValidationFailure)
                return new HttpResponseMessage(HttpStatusCode.BadRequest);


            return new HttpResponseMessage(HttpStatusCode.OK);

        }
    }
}