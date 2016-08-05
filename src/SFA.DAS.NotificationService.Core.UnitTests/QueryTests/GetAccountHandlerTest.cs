using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SFA.DAS.NotificationService.Application.Queries.GetAccount;
using SFA.DAS.NotificationService.Domain.Models;
using SFA.DAS.NotificationService.Domain.Repositories;

namespace SFA.DAS.NotificationService.Application.UnitTests.QueryTests
{
    [TestClass]
    public class GetAccountHandlerTest
    {
        private GetAccountHandler _sut;
        private Mock<IAccountRepository> _accountRepositoryMock;

        [TestInitialize]
        public void Init()
        {
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _sut = new GetAccountHandler(_accountRepositoryMock.Object);
        }

        [TestMethod]
        public void ShouldGetAccount()
        {
            // Assign
            var account = new Account
            {
                Id = 1,
                Name = "Test Account"
            };
            _accountRepositoryMock.Setup(x => x.Get(account.Id)).ReturnsAsync(account);

            // Act
            var result = _sut.Handle(new GetAccountRequest {AccountId = account.Id});

            result.Wait(500);

            // Assert
            result.Result.Account.Should().Be(account);
            _accountRepositoryMock.Verify(x => x.Get(account.Id), Times.Once);
        }

        [TestMethod]
        public void ShouldReturnNullIfCannotGetAccount()
        {
            // Assign
            _accountRepositoryMock.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync(null);

            // Act
            var result = _sut.Handle(new GetAccountRequest {AccountId = 1});

            result.Wait(500);

            // Assert
            result.Result.Should().BeNull();
            _accountRepositoryMock.Verify(x => x.Get(It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public void ShouldThrowExceptionIfRequestIdIsInvalid()
        {
            // Assign
            _accountRepositoryMock.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync(null);

            // Act
            Action act = () => _sut.Handle(new GetAccountRequest { AccountId = -1 }).Wait(500);

            act.ShouldThrow<ArgumentException>();
        }

        [TestMethod]
        public void ShouldNotGetAccountIfRequestIsNull()
        {
            // Assign
            _accountRepositoryMock.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync(null);

            // Act
            Action act = () => _sut.Handle(null).Wait(500);

            act.ShouldThrow<ArgumentNullException>();
        }
    }
}
