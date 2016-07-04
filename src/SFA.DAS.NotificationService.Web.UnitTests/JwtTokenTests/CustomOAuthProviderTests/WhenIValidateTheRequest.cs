using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using NUnit.Framework;
using SFA.DAS.NotificationService.Application;

namespace SFA.DAS.NotificationService.Api.UnitTests.JwtTokenTests.CustomOAuthProviderTests
{
    public class WhenIValidateTheRequest
    {
        private CustomOAuthProvider _customOauthProvider;

        [SetUp]
        public void Arrange()
        {
            _customOauthProvider = new CustomOAuthProvider(new PortalJwtTokenConfiguration { AudienceId = "audience" });
        }

        [Test]
        public async Task ThenTheContextIsValidated()
        {
            //Arrange
            var context = new OAuthValidateClientAuthenticationContext(new OwinContext(), new OAuthAuthorizationServerOptions(), null);

            //Act
            await _customOauthProvider.ValidateClientAuthentication(context);

            //Assert
            Assert.IsTrue(context.IsValidated);
        }

        [Test]
        public async Task ThenAServerErrorIsAddedToTheContextWhenThereAreNoParametes()
        {
            //Arrange
            var context = new OAuthValidateClientAuthenticationContext(new OwinContext(), new OAuthAuthorizationServerOptions(), null);

            //Act
            await _customOauthProvider.ValidateClientAuthentication(context);

            //Assert
            Assert.AreEqual("Server error", context.Error);
        }

        [Test]
        public async Task ThenTheUserNameAndPasswordAreChecked()
        {
            //Arrange
            var formCollection = new FormCollection(new Dictionary<string, string[]>
            {
                { "username", new [] {"test"}},
                { "password", new [] {"tester"}}
            });
            var context = new OAuthValidateClientAuthenticationContext(new OwinContext(), new OAuthAuthorizationServerOptions(), formCollection);


            //Act
            await _customOauthProvider.ValidateClientAuthentication(context);

            //Assert
            Assert.IsTrue(context.IsValidated);
            Assert.AreEqual("Invalid credentials", context.Error);
        }

        [Test]
        public async Task ThenTheUserNameIsAddedToTheOWinContextWhenCorrectlyValidated()
        {
            //Arrange

            var formCollection = new FormCollection(new Dictionary<string, string[]>
            {
                { "username", new [] {"test"}},
                { "password", new [] {"test"}}
            });
            var context = new OAuthValidateClientAuthenticationContext(new OwinContext(), new OAuthAuthorizationServerOptions(), formCollection);

            //Act
            await _customOauthProvider.ValidateClientAuthentication(context);

            //Assert
            Assert.AreEqual("test", context.OwinContext.Get<string>("otc:username"));
        }



        [Test]
        public async Task ThenTheUserNameIsNotAddedToTheOWinContextWhenNotValid()
        {
            //Arrange

            var formCollection = new FormCollection(new Dictionary<string, string[]>
            {
                { "username", new [] {"test"}},
                { "password", new [] {"tester"}}
            });
            var context = new OAuthValidateClientAuthenticationContext(new OwinContext(), new OAuthAuthorizationServerOptions(), formCollection);

            //Act
            await _customOauthProvider.ValidateClientAuthentication(context);

            //Assert
            Assert.IsNull(context.OwinContext.Get<string>("otc:username"));
        }
    }
}
