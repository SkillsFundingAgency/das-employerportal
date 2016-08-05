using System.Net;
using FluentAssertions;
using MediatR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using SFA.DAS.NotificationService.Api.Controllers;
using SFA.DAS.NotificationService.Application.Queries.GetAccount;
using SFA.DAS.NotificationService.Domain.Models;

namespace SFA.DAS.NotificationService.Api.UnitTests.ControllerTests
{
    [TestClass]
    public class AccountControllerTests
    {
        private AccountController _sut;
        private Mock<IMediator> _mediatorMock;

        [TestInitialize]
        public void Init()
        {
            _mediatorMock = new Mock<IMediator>();
            _sut = new AccountController(_mediatorMock.Object);
        }

        [TestMethod]
        public void ShouldGetAccount()
        {
            // Assign
            var response = new GetAccountResponse
            {
                Account = new Account
                {
                    Id = 2,
                    Name = "Test Account"
                }
            };

            _mediatorMock.Setup(x => x.SendAsync(It.IsAny<GetAccountRequest>())).ReturnsAsync(response);

            var expectedContent = JsonConvert.SerializeObject(response.Account);

            // Act
            var result = _sut.Get(response.Account.Id);

            // Assert
            _mediatorMock.Verify(x => x.SendAsync(It.IsAny<GetAccountRequest>()), Times.Once);
            result.Result.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Result.Content.ReadAsStringAsync().Result.Should().Be(expectedContent);
        }

        [TestMethod]
        public void ShouldReturnCannotFindAccount()
        {
            // Assign
            _mediatorMock.Setup(x => x.SendAsync(It.IsAny<GetAccountRequest>())).ReturnsAsync(null);
            
            // Act
            var result = _sut.Get(1);

            // Assert
            _mediatorMock.Verify(x => x.SendAsync(It.IsAny<GetAccountRequest>()), Times.Once);
            result.Result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [TestMethod]
        public void ShouldReturnBadRequestIfIdInvalid()
        {
            // Assign
            _mediatorMock.Setup(x => x.SendAsync(It.IsAny<GetAccountRequest>())).ReturnsAsync(null);

            // Act
            var result = _sut.Get(0);

            // Assert
            _mediatorMock.Verify(x => x.SendAsync(It.IsAny<GetAccountRequest>()), Times.Never);
            result.Result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
