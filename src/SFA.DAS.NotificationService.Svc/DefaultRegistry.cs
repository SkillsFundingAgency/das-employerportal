using System;
using FluentValidation;
using MediatR;
using Microsoft.WindowsAzure;
using SFA.DAS.Configuration;
using SFA.DAS.Configuration.AzureTableStorage;
using SFA.DAS.Messaging;
using SFA.DAS.Messaging.AzureServiceBus;
using SFA.DAS.Messaging.FileSystem;
using SFA.DAS.NotificationService.Application;
using SFA.DAS.NotificationService.Application.Interfaces;
using SFA.DAS.NotificationService.Worker.EmailServices;
using StructureMap.Configuration.DSL;

namespace SFA.DAS.NotificationService.Worker
{
    public class DefaultRegistry : Registry
    {
        private const string ServiceName = "SFA.DAS.NotificationService";
        private const string DevEnv = "LOCAL";
        private const string CloudDevEnv = "CLOUD_DEV";

        public DefaultRegistry()
        {

            var environment = Environment.GetEnvironmentVariable("DASENV");
            if (string.IsNullOrEmpty(environment))
            {
                environment = CloudConfigurationManager.GetSetting("EnvironmentName");
            }
            
            var connectionString = CloudConfigurationManager.GetSetting("StorageConnectionString");

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
            For<IConfigurationRepository>().Use<AzureTableStorageConfigurationRepository>().Ctor<string>().Is(connectionString);
            var configurationService = new ConfigurationService(new AzureTableStorageConfigurationRepository(connectionString),
                new ConfigurationOptions(ServiceName, environment, "1.0"));
            For<IConfigurationService>().Use(configurationService);
            For<IMessageNotificationRepository>().Use<AzureEmailNotificationRepository>().Ctor<string>().Is(connectionString);

            if (environment == DevEnv)
            {
                For<IMessageSubSystem>().Use(() => new FileSystemMessageSubSystem());
                For<IEmailService>().Use<LocalEmailService>();
                //For<IEmailService>().Use<SendGridSmtpEmailService>();
            }
            else
            {
                var config = configurationService.Get<NotificationServiceConfiguration>().Result;
                var queueConfig = config.ServiceBusConfiguration;
                For<IMessageSubSystem>().Use(() => new AzureServiceBusMessageSubSystem(queueConfig.ConnectionString, queueConfig.QueueName));
                For<IEmailService>().Use<SendGridSmtpEmailService>();
            }
            
            For<MessagingService>().Use<MessagingService>();
            For<QueuedMessageHandler>().Use<QueuedMessageHandler>();
            For<IMediator>().Use<Mediator>();
        }
    }
}