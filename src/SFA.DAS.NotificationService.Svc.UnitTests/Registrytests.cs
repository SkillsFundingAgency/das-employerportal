using System;
using NUnit.Framework;
using StructureMap;

namespace SFA.DAS.NotificationService.Worker.UnitTests
{
    [TestFixture]
    public class RegistryTests
    {
        [Test]
        public void VerifyRegistry()
        {
            Environment.SetEnvironmentVariable("DASENV", "LOCAL");

            var registry = new DefaultRegistry();

            var container = new Container(registry);

            container.AssertConfigurationIsValid();
        }
    }
}
