using NUnit.Framework;
using StructureMap;

namespace SFA.DAS.NotificationService.Worker.UnitTests
{
    [TestFixture]
    public class RegistryTests
    {
        [Ignore("Issue with Azure Configuration")]
        [Test]
        public void VerifyRegistry()
        {
            var registry = new DefaultRegistry();

            var container = new Container(registry);

            container.AssertConfigurationIsValid();
        }
    }
}
