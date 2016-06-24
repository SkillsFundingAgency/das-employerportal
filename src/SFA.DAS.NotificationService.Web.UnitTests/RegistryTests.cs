using System;
using NUnit.Framework;
using SFA.DAS.NotificationService.Api.Controllers;
using SFA.DAS.NotificationService.Api.DependencyResolution;
using SFA.DAS.NotificationService.Api.Orchestrators;
using StructureMap;

namespace SFA.DAS.NotificationService.Api.UnitTests
{
    [TestFixture]
    public class RegistryTests
    {
        private DefaultRegistry _registry;
        private string _currentVariable;

        [SetUp]
        public void Setup()
        {
            _currentVariable = Environment.GetEnvironmentVariable("DASENV");

            Environment.SetEnvironmentVariable("DASENV", "LOCAL");

            _registry = new DefaultRegistry();
        }

        [TearDown]
        public void Teardown()
        {
            Environment.SetEnvironmentVariable("DASENV", _currentVariable);
        }

        [Test]
        public void Test()
        {
            var container = new Container(_registry);

            container.AssertConfigurationIsValid();
        }

        [Test]
        public void CreateNotificationController()
        {
            var container = new Container(_registry);

            var controller = container.GetInstance<EmailController>();

            Assert.That(controller, Is.Not.Null);
        }

        [Test]
        public void CreateNotificationOrchestrator()
        {
            var container = new Container(_registry);

            var orchestrator = container.GetInstance<NotificationOrchestrator>();

            Assert.That(orchestrator, Is.Not.Null);
        }
    }
}
