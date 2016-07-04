using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using NUnit.Framework;
using SFA.DAS.NotificationService.Application;

namespace SFA.DAS.NotificationService.Api.UnitTests.JwtTokenTests.CustomOAuthProviderTests
{
    public class WhenIGrantResourceOwnerCredentials
    {
        private CustomOAuthProvider _customOauthProvider;
        private OAuthGrantResourceOwnerCredentialsContext _context;

        [SetUp]
        public void Arrange()
        {
            _customOauthProvider = new CustomOAuthProvider(new PortalJwtTokenConfiguration {AudienceId = "audience_id"});

            var owinContext = new OwinContext();
            owinContext.Set("otc:username", "test");
            _context = new OAuthGrantResourceOwnerCredentialsContext(owinContext, new OAuthAuthorizationServerOptions(), "", "test", "test", new List<string>());
        }

        [Test]
        public async Task ThenTheIfTheCredentialsAreIncorrectAnErrorWillBeSet()
        {
            //Arrange
            var owinContext = new OwinContext();
            var context = new OAuthGrantResourceOwnerCredentialsContext(owinContext,new OAuthAuthorizationServerOptions(), "","test","tester",new List<string>() );

            //Act
            await _customOauthProvider.GrantResourceOwnerCredentials(context);

            //Assert
            Assert.IsTrue(context.HasError);
            Assert.AreEqual("invalid_grant", context.Error);
            Assert.AreEqual("The user name or password is incorrect", context.ErrorDescription);
        }

        [Test]
        public async Task ThenTheIfTheCredentialsAreIncorrectTheAuthTicketIsNotSet()
        {
            //Arrange
            var owinContext = new OwinContext();
            var context = new OAuthGrantResourceOwnerCredentialsContext(owinContext, new OAuthAuthorizationServerOptions(), "", "test", "tester", new List<string>());

            //Act
            await _customOauthProvider.GrantResourceOwnerCredentials(context);

            //Assert
            Assert.IsTrue(context.HasError);
            Assert.IsNull( context.Ticket);
        }

        [Test]
        public async Task ThenThContextIsValidatedIfCredentialsAreCorrect()
        {
            //Act
            await _customOauthProvider.GrantResourceOwnerCredentials(_context);

            //Assert
            Assert.IsFalse(_context.HasError);
            Assert.IsTrue(_context.IsValidated);
        }

        [Test]
        public async Task ThenTheAuthenticationTicketContainsClaimsWhenValid()
        {
            //Act
            await _customOauthProvider.GrantResourceOwnerCredentials(_context);

            //Assert
            var claimsIdentity = _context.Ticket.Identity;
            Assert.IsNotNull(claimsIdentity);
            Assert.AreEqual("test", claimsIdentity.Claims.Where(c=>c.Type=="sub").Select(x=>x.Value).First());

        }

        [Test]
        public async Task ThenTheAudienceIdIsAddedFromConfig()
        {
            //Act
            await _customOauthProvider.GrantResourceOwnerCredentials(_context);

            //Assert
            Assert.Contains(new KeyValuePair<string,string>("audience", "audience_id"), _context.Ticket.Properties.Dictionary.ToDictionary(c=>c.Key,x=>x.Value));
            
        }
    }
}
