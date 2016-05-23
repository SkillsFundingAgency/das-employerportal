using MediatR;
using NUnit.Framework;
using SFA.DAS.NotificationService.Web.Controllers;
using SFA.DAS.NotificationService.Web.DependencyResolution;
using SFA.DAS.NotificationService.Web.Orchestrators;
using StructureMap;

namespace SFA.DAS.NotificationService.Web.UnitTests
{
    [TestFixture]
    public class RegistryTests
    {
        [Test]
        public void Test()
        {
            var registry = new DefaultRegistry();

            var container = new Container(registry);

            container.AssertConfigurationIsValid();
        }

        [Test]
        public void CreateNotificationController()
        {
            var registry = new DefaultRegistry();

            var container = new Container(registry);

            var controller = container.GetInstance<NotificationController>();

            Assert.That(controller, Is.Not.Null);
        }

        [Test]
        public void CreateNotificationOrchestrator()
        {
            var registry = new DefaultRegistry();

            var container = new Container(registry);

            var orchestrator = container.GetInstance<NotificationOrchestrator>();

            Assert.That(orchestrator, Is.Not.Null);
        }

        //[Test]
        //public void CreateSendEmailCommandHandler()
        //{
        //    var registry = new DefaultRegistry();

        //    var container = new Container(registry);

        //    var mediator = container.GetInstance<IMediator>();

        //    var cmd = mediator.Send(new SendEmailCommand());

        //    Assert.That(cmd, Is.Not.Null);
        //}
    }
}
