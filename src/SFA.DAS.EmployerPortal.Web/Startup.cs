using Microsoft.Owin;
using NLog;
using Owin;
using SFA.DAS.Configuration;
using SFA.DAS.EmployerPortal.Infrastructure.Configuration;

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
            configurationService.GetAsync<EmployerPortalConfiguration>().ContinueWith((task) =>
            {
                if (task.Exception != null)
                {
                    throw task.Exception;
                }

                var configuration = task.Result;
                ConfigureRelyingParty(app, configuration.IdentifyingParty);
            }).Wait();
        }
    }
}