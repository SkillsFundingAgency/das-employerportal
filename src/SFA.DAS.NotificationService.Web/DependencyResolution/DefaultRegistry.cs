// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultRegistry.cs" company="Web Advanced">
// Copyright 2012 Web Advanced (www.webadvanced.com)
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Configuration;
using System.Web.WebPages;
using FluentValidation;
using MediatR;
using Microsoft.WindowsAzure;
using SFA.DAS.Configuration;
using SFA.DAS.Configuration.AzureTableStorage;
using SFA.DAS.Configuration.FileStorage;
using SFA.DAS.Messaging;
using SFA.DAS.Messaging.AzureServiceBus;
using SFA.DAS.Messaging.FileSystem;
using SFA.DAS.NotificationService.Api.Orchestrators;
using SFA.DAS.NotificationService.Application;
using SFA.DAS.NotificationService.Application.Interfaces;
using SFA.DAS.NotificationService.Domain.Repositories;
using SFA.DAS.NotificationService.Infrastructure;
using SFA.DAS.NotificationService.Infrastructure.Repositories;
using StructureMap.Configuration.DSL;

namespace SFA.DAS.NotificationService.Api.DependencyResolution {
	
    public class DefaultRegistry : Registry {
        private const string ServiceName = "SFA.DAS.NotificationService";
        private const string DevEnv = "LOCAL";

        public DefaultRegistry() {

            var environment = Environment.GetEnvironmentVariable("DASENV");
            if (string.IsNullOrEmpty(environment))
            {
                environment = CloudConfigurationManager.GetSetting("EnvironmentName");
            }
            
            Scan(
                scan => {
                    scan.WithDefaultConventions();
                    scan.With(new ControllerConvention());
                    scan.AssemblyContainingType<IMessageNotificationRepository>();
                    scan.ConnectImplementationsToTypesClosing(typeof(IRequestHandler<,>));
                    scan.ConnectImplementationsToTypesClosing(typeof(IAsyncRequestHandler<,>));
                    scan.ConnectImplementationsToTypesClosing(typeof(INotificationHandler<>));
                    scan.ConnectImplementationsToTypesClosing(typeof(IAsyncNotificationHandler<>));
                    scan.ConnectImplementationsToTypesClosing(typeof(AbstractValidator<>));
                });
            For<SingleInstanceFactory>().Use<SingleInstanceFactory>(ctx => t => ctx.GetInstance(t));
            For<MultiInstanceFactory>().Use<MultiInstanceFactory>(ctx => t => ctx.GetAllInstances(t));

            IConfigurationRepository configurationRepository;

            if (ConfigurationManager.AppSettings["LocalConfig"].AsBool())
            {
                configurationRepository = new FileStorageConfigurationRepository();
            }
            else
            {
                configurationRepository = new AzureTableStorageConfigurationRepository(CloudConfigurationManager.GetSetting("ConfigurationStorageConnectionString"));
            }


            var configurationService = new ConfigurationService(
                configurationRepository,
                new ConfigurationOptions(ServiceName, environment, "1.0"));
            For<IConfigurationService>().Use(configurationService);
            if (environment == DevEnv)
            {
                For<IMessageSubSystem>().Use(() => new FileSystemMessageSubSystem());
            }
            else
            {
                var config = configurationService.Get<NotificationServiceConfiguration>();
                var queueConfig = config.ServiceBusConfiguration;
                For<IMessageSubSystem>().Use(() => new AzureServiceBusMessageSubSystem(queueConfig.ConnectionString, queueConfig.QueueName));
            }

            For<MessagingService>().Use<MessagingService>();

            var storageConnectionString = CloudConfigurationManager.GetSetting("StorageConnectionString") ??
                              "UseDevelopmentStorage=true";

            For<IMessageNotificationRepository>().Use<AzureEmailNotificationRepository>().Ctor<string>().Is(storageConnectionString);
            For<INotificationOrchestrator>().Use<NotificationOrchestrator>();
            For<IAccountRepository>().Use<AccountRepository>();
            For<IMediator>().Use<Mediator>();
        }
    }
}