using System.Net;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.NotificationService.Api.Controllers;
using SFA.DAS.NotificationService.Api.Core;
using SFA.DAS.NotificationService.Api.Models;
using SFA.DAS.NotificationService.Api.Orchestrators;

namespace SFA.DAS.NotificationService.Api.UnitTests.ControllerTests
{
    [TestFixture]
    public class EmailControllerTests
    {
        private Mock<INotificationOrchestrator> _orchestrator;
        private EmailController _controller;

        [SetUp]
        public void Setup()
        {
            _orchestrator = new Mock<INotificationOrchestrator>();

            _controller = new EmailController(_orchestrator.Object);
        }

        [Test]
        public async Task ReturnBadRequestOnValidationFailure()
        {
            var response = new OrchestratorResponse
            {
                Code = NotificationOrchestratorCodes.Post.ValidationFailure
            };

            _orchestrator.Setup(x => x.SendEmail(It.IsAny<EmailViewModel>())).ReturnsAsync(response);

            var httpResponse = await _controller.Post(new EmailViewModel());

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task ReturnSuccessOKOnMessageSent()
        {
            var response = new OrchestratorResponse
            {
                Code = NotificationOrchestratorCodes.Post.Success
            };

            _orchestrator.Setup(x => x.SendEmail(It.IsAny<EmailViewModel>())).ReturnsAsync(response);

            var httpResponse = await _controller.Post(new EmailViewModel());

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
    }
}