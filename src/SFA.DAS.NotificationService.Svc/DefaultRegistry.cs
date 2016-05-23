using FluentValidation;
using MediatR;
using SFA.DAS.Configuration;
using SFA.DAS.Configuration.AzureTableStorage;
using SFA.DAS.Messaging;
using SFA.DAS.Messaging.FileSystem;
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
                    scan.AssemblyContainingType<IMessageNotificationRepository>();
                    scan.ConnectImplementationsToTypesClosing(typeof(IRequestHandler<,>));
                    scan.ConnectImplementationsToTypesClosing(typeof(IAsyncRequestHandler<,>));
                    scan.ConnectImplementationsToTypesClosing(typeof(INotificationHandler<>));
                    scan.ConnectImplementationsToTypesClosing(typeof(IAsyncNotificationHandler<>));
                    scan.ConnectImplementationsToTypesClosing(typeof(AbstractValidator<>));
                });
            For<SingleInstanceFactory>().Use<SingleInstanceFactory>(ctx => t => ctx.GetInstance(t));
            For<MultiInstanceFactory>().Use<MultiInstanceFactory>(ctx => t => ctx.GetAllInstances(t));
            For<IConfigurationRepository>().Use<AzureTableStorageConfigurationRepository>().Ctor<string>().Is("UseDevelopmentStorage=true");
            var configurationService = new ConfigurationService(new AzureTableStorageConfigurationRepository("UseDevelopmentStorage=true"),
                new ConfigurationOptions("SFA.DAS.NotificationService.Svc", null, "1.0"));
            For<IConfigurationService>().Use(configurationService);
            For<IMessageNotificationRepository>().Use<AzureEmailNotificationRepository>().Ctor<string>().Is("UseDevelopmentStorage=true");
            For<IMessageSubSystem>().Use(() => new FileSystemMessageSubSystem());
            For<MessagingService>().Use<MessagingService>();
            For<QueuedMessageHandler>().Use<QueuedMessageHandler>();
            For<IEmailService>().Use<LocalEmailService>();
            For<IMediator>().Use<Mediator>();
        }
    }
}