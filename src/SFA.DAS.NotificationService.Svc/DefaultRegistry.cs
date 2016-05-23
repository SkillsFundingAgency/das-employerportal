using SFA.DAS.Configuration;
using SFA.DAS.Configuration.AzureTableStorage;
using SFA.DAS.NotificationService.Application;
using SFA.DAS.NotificationService.Application.Interfaces;
using StructureMap.Configuration.DSL;

namespace SFA.DAS.NotificationService.Worker
{
    public class DefaultRegistry : Registry
    {
        public DefaultRegistry()
        {
            Scan(
                scan => {
                    scan.WithDefaultConventions();
                });
            For<IConfigurationRepository>().Use<AzureTableStorageConfigurationRepository>().Ctor<string>().Is("UseDevelopmentStorage=true");
            var configurationService = new ConfigurationService(new AzureTableStorageConfigurationRepository("UseDevelopmentStorage=true"),
                new ConfigurationOptions("SFA.DAS.NotificationService.Svc", null, "1.0"));
            For<IConfigurationService>().Use(configurationService);
            For<IMessageNotificationRepository>().Use<AzureEmailNotificationRepository>().Ctor<string>().Is("UseDevelopmentStorage=true");
        }
    }
}