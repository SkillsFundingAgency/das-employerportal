using Microsoft.Owin;
using NLog;
using Owin;

[assembly: OwinStartup(typeof(SFA.DAS.NotificationService.Web.Startup))]

namespace SFA.DAS.NotificationService.Web
{
    public partial class Startup
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public void Configuration(IAppBuilder app)
        {
            _logger.Debug("Started running Owin Configuration");

            ConfigureAuth(app);
        }
    }
}
