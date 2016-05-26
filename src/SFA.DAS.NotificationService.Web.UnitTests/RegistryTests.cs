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
        [Ignore("Issue with Azure Configuration")]
        [Test]
        public void Test()
        {
            var registry = new DefaultRegistry();

            var container = new Container(registry);

            container.AssertConfigurationIsValid();
        }

        [Ignore("Issue with Azure Configuration")]
        [Test]
        public void CreateNotificationController()
        {
            var registry = new DefaultRegistry();

            var container = new Container(registry);

            var controller = container.GetInstance<NotificationController>();

            Assert.That(controller, Is.Not.Null);
        }

        [Ignore("Issue with Azure Configuration")]
        [Test]
        public void CreateNotificationOrchestrator()
        {
            var registry = new DefaultRegistry();

            var container = new Container(registry);

            var orchestrator = container.GetInstance<NotificationOrchestrator>();

            Assert.That(orchestrator, Is.Not.Null);
        }
    }
}
