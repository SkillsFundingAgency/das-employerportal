﻿using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using SFA.DAS.Messaging;
using SFA.DAS.NotificationService.Core;
using SFA.DAS.NotificationService.Core.Messages;
using SFA.DAS.NotificationService.Web.Models;
using SFA.DAS.NotificationService.Web.Orchestrators;
using SFA.DAS.TimeProvider;

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

        public async Task<HttpResponseMessage> Post(EmailNotification notification)
        {
            return await Task.Run<HttpResponseMessage>(() =>
            {
                var response = _orchestrator.Post(notification);

                return new HttpResponseMessage(HttpStatusCode.OK);
            });
        }
    }
}
