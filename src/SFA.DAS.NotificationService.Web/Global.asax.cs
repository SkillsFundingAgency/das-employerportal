using System.Web.Http;

namespace SFA.DAS.NotificationService.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            LoggingConfig.ConfigureLogging();
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
