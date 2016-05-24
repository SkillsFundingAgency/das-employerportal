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
using Microsoft.WindowsAzure.ServiceRuntime;
using SFA.DAS.Configuration;
using SFA.DAS.Configuration.AzureTableStorage;

namespace SFA.DAS.EmployerPortal.Web.DependencyResolution {
    using StructureMap.Configuration.DSL;
    using StructureMap.Graph;
	
    public class DefaultRegistry : Registry {
        #region Constructors and Destructors
        private const string ServiceName = "SFA.DAS.NotificationService";
        private const string DevEnv = "DEV";
        private const string CloudDevEnv = "CLOUD_DEV";
        public DefaultRegistry() {

            var environmentName = DevEnv ;

            var connectionString = CloudConfigurationManager.GetSetting("StorageConnectionString");

            Scan(
                scan => {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
					scan.With(new ControllerConvention());
                });
            //For<IExample>().Use<Example>();

            For<IConfigurationRepository>().Use<AzureTableStorageConfigurationRepository>().Ctor<string>().Is(connectionString);
            var configurationService = new ConfigurationService(new AzureTableStorageConfigurationRepository(connectionString),
                new ConfigurationOptions(ServiceName, environmentName, "1.0"));
            For<IConfigurationService>().Use(configurationService);
        }

        #endregion
    }
}