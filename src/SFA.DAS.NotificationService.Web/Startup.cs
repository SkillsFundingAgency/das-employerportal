using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using NLog;
using Owin;
using SFA.DAS.Configuration;
using SFA.DAS.NotificationService.Api.DependencyResolution;
using SFA.DAS.NotificationService.Application;
using SFA.DAS.NotificationService.Application.Extensions;

[assembly: OwinStartup(typeof(SFA.DAS.NotificationService.Api.Startup))]
namespace SFA.DAS.NotificationService.Api
{
    public class Startup
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public void Configuration(IAppBuilder app)
        {
            Logger.Debug("Started running Owin Configuration for API");

            var container = IoC.Initialize();
            var config = new HttpConfiguration
            {
                DependencyResolver = new StructureMapWebApiDependencyResolver(container)
            };

            WebApiConfig.Register(config);

            var configurationService = container.GetInstance<IConfigurationService>();

            configurationService.GetAsync<NotificationServiceConfiguration>().ContinueWith(task =>
                {
                    if (task.Exception != null)
                    {
                        task.Exception.UnpackAndLog(Logger);
                        throw task.Exception.InnerExceptions[0];
                    }
                    var configuration = task.Result;
                    ConfigureAuth(app, configuration.PortalJwtToken);
                }
                ).Wait();


            //app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);
        }

        public void ConfigureAuth(IAppBuilder app, PortalJwtTokenConfiguration config)
        {
            var issuer = config.Issuer;
            var oAuthServerOptions = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = config.AllowInsecureHttp,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(config.AccessTokenExpiryTimeInMinutes),
                Provider = new CustomOAuthProvider(config),
                AccessTokenFormat = new CustomJwtFormat(config)
            };

            app.UseOAuthAuthorizationServer(oAuthServerOptions);

            var audience = config.AudienceId;
            var secret = TextEncodings.Base64Url.Decode(config.Secret);

            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions
                {
                    AuthenticationMode = AuthenticationMode.Active,
                    AllowedAudiences = new[] { audience },
                    IssuerSecurityTokenProviders = new IIssuerSecurityTokenProvider[]
                    {
                        new SymmetricKeyIssuerSecurityTokenProvider(issuer, secret)
                    },

                    Provider = new OAuthBearerAuthenticationProvider
                    {
                        OnApplyChallenge = async context =>
                        {

                            var authHeader = context.Request.Headers.GetValues("Authorization");

                            if ((authHeader == null || !authHeader.Any()) || !authHeader.Any(c => c.StartsWith("bearer", StringComparison.CurrentCultureIgnoreCase)))
                            {
                                Logger.Warn("Missing Authorization Bearer Token");
                                
                                context.Response.StatusCode = 400;
                            }

                            await Task.FromResult<object>(null);
                        },
                        OnValidateIdentity = context =>
                        {
                            context.Ticket.Identity.AddClaim(new System.Security.Claims.Claim("newCustomClaim", "newValue"));
                            return Task.FromResult<object>(null);
                        }
                    }
                });

        }
    }
}