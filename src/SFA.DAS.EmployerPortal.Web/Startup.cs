using Microsoft.Owin;
using NLog;
using Owin;
using SFA.DAS.Configuration;
using SFA.DAS.EmployerPortal.Web.Models;

[assembly: OwinStartup(typeof(SFA.DAS.EmployerPortal.Web.Startup))]
namespace SFA.DAS.EmployerPortal.Web
{
    
    public partial class Startup
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public void Configuration(IAppBuilder app)
        {
            _logger.Debug("Started running Owin Configuration");

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