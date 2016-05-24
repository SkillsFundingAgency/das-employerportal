using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Owin;
using SFA.DAS.Configuration;
using SFA.DAS.EmployerPortal.Web.Models;

[assembly: OwinStartup(typeof(SFA.DAS.EmployerPortal.Web.Startup))]
namespace SFA.DAS.EmployerPortal.Web
{
    
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {

            var configurationService = StructuremapMvc.Container.GetInstance<IConfigurationService>();
            configurationService.Get<EmployerPortalConfiguration>().ContinueWith((task) =>
            {
                if (task.Exception != null)
                {
                    throw task.Exception;
                }

                var configuration = task.Result;
                ConfigureRelyingParty(app, configuration);
            }).Wait();
        }
    }
}