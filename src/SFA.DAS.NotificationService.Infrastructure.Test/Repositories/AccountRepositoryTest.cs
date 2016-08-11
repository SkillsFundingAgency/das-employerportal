using System.Configuration;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Configuration;
using SFA.DAS.NotificationService.Application;
using SFA.DAS.NotificationService.Domain.Models;
using SFA.DAS.NotificationService.Infrastructure.Repositories;

namespace SFA.DAS.NotificationService.Infrastructure.Test.Repositories
{
    /// <summary>
    /// These tests are here to test the full functionaility of the repository as we currently don't use 
    /// all the methods of the repository. These test will later on be replaced by full scale intergration
    /// test that will test end to end. These tests are ignored by default.
    /// </summary>
    [TestFixture]
    [Ignore("This test is mainly to test the repository code locally as we currently don't have a full integration test yet")]
    public class AccountRepositoryTest
    {
        private Account _account;
        private AccountRepository _repository;
        private NotificationServiceConfiguration _configuration;
        private Mock<IConfigurationService> _configurationServiceMock;


        [SetUp]
        public void Init()
        {
            _configuration = new NotificationServiceConfiguration
            {
                LevyDatabase = new LevyDatabaseConfiguration
                {
                    ConnectionString = ConfigurationManager.ConnectionStrings["DAS_AML"].ConnectionString
                }
            };

            _configurationServiceMock = new Mock<IConfigurationService>();
            _configurationServiceMock.Setup(x => x.GetAsync<NotificationServiceConfiguration>()).ReturnsAsync(_configuration);

            _account = new Account
            {
                Id = 500,
                Name = "Test Account"
            };

            _repository = new AccountRepository(_configurationServiceMock.Object);
        }

        [Test]
        public async Task ShouldBeAbleToCreateAndGetAndDeleteAnAccount()
        {
            // Save the account
            _account.Id = await _repository.Save(_account);

            // Test both the save and get code to make sure the account can be retrieved from the database
            var result = await _repository.Get(_account.Id);
            result.Id.Should().Be(_account.Id);
            result.Name.Trim().Should().Be(_account.Name);

            // Delete the account
            await _repository.Delete(_account);

            // Make sure the account is no longer in the database (it has been deleted)
            result = await _repository.Get(_account.Id);
            result.Should().BeNull();
        }
    }
}