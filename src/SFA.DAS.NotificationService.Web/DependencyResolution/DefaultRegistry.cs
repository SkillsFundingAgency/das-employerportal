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

using Microsoft.WindowsAzure;
using SFA.DAS.Configuration;
using SFA.DAS.Configuration.AzureTableStorage;
using SFA.DAS.Messaging;
using SFA.DAS.Messaging.FileSystem;
using SFA.DAS.NotificationService.Application;
using SFA.DAS.NotificationService.Web.Orchestrators;
using StructureMap.Configuration.DSL;


namespace SFA.DAS.NotificationService.Web.DependencyResolution {
	
    public class DefaultRegistry : Registry {
        #region Constructors and Destructors

        public DefaultRegistry() {
            Scan(
                scan => {
                    scan.WithDefaultConventions();
                    scan.With(new ControllerConvention());
                });
            For<IConfigurationRepository>().Use<AzureTableStorageConfigurationRepository>().Ctor<string>().Is("UseDevelopmentStorage=true");
            var configurationService = new ConfigurationService(new AzureTableStorageConfigurationRepository("UseDevelopmentStorage=true"),
                new ConfigurationOptions("SFA.DAS.NotificationService.Web", null, "1.0"));
            For<IConfigurationService>().Use(configurationService);
            For<IMessageSubSystem>().Use(() => new FileSystemMessageSubSystem());
            For<MessagingService>().Use<MessagingService>();
            For<IEmailNotificationRepository>().Use<AzureEmailNotificationRepository>().Ctor<string>().Is("UseDevelopmentStorage=true");
            For<INotificationOrchestrator>().Use<NotificationOrchestrator>();
        }

        #endregion
    }
}