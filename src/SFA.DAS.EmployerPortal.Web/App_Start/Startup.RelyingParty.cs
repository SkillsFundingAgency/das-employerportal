using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using SFA.DAS.EmployerPortal.Infrastructure.Configuration;
using SFA.DAS.EmployerUsers.WebClientComponents;
using Thinktecture.IdentityModel.Client;

namespace SFA.DAS.EmployerPortal.Web
{
    public partial class Startup
    {
        private void ConfigureRelyingParty(IAppBuilder app, IdentifyingPartyConfiguration configuration)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "Cookies"
            });

            var idpUrl = configuration.ApplicationBaseUrl.EndsWith("/")
                ? configuration.ApplicationBaseUrl + "identity/"
                : configuration.ApplicationBaseUrl + "/identity/";
            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                Authority = idpUrl,

                ClientId = "employerportal",
                Scope = "openid profile",
                ResponseType = "id_token token",
                RedirectUri = configuration.LoginReturnUrl,

                SignInAsAuthenticationType = "Cookies",
                UseTokenLifetime = false,


                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    SecurityTokenValidated = async n =>
                    {
                        var nid = new ClaimsIdentity(
                            n.AuthenticationTicket.Identity.AuthenticationType,
                            DasClaimTypes.DisplayName,
                            "role");

                        // get userinfo data
                        var userInfoClient = new UserInfoClient(
                            new Uri(n.Options.Authority + "/connect/userinfo"),
                            n.ProtocolMessage.AccessToken);

                        var userInfo = await userInfoClient.GetAsync();
                        userInfo.Claims.ToList().ForEach(ui => nid.AddClaim(new Claim(ui.Item1, ui.Item2)));

                        // keep the id_token for logout
                        nid.AddClaim(new Claim("id_token", n.ProtocolMessage.IdToken));

                        // add access token for sample API
                        nid.AddClaim(new Claim("access_token", n.ProtocolMessage.AccessToken));

                        // keep track of access token expiration
                        nid.AddClaim(new Claim("expires_at", DateTimeOffset.Now.AddSeconds(int.Parse(n.ProtocolMessage.ExpiresIn)).ToString()));

                        n.AuthenticationTicket = new AuthenticationTicket(
                            nid,
                            n.AuthenticationTicket.Properties);
                    },

                    RedirectToIdentityProvider = n =>
                    {
                        // Do not fiddle with api request please.
                        if (n.Request.Uri.AbsolutePath.ToLower().StartsWith("/api/"))
                        {
                            n.HandleResponse();
                            return Task.FromResult(0);
                        }

                        if (n.ProtocolMessage.RequestType == OpenIdConnectRequestType.LogoutRequest)
                        {
                            var idTokenHint = n.OwinContext.Authentication.User.FindFirst("id_token");

                            if (idTokenHint != null)
                            {
                                n.ProtocolMessage.IdTokenHint = idTokenHint.Value;
                            }
                        }

                        return Task.FromResult(0);
                    }
                }
            });
        }
    }
}