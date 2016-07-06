using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.NotificationService.Application.Messages;
using SFA.DAS.NotificationService.Infrastructure.Notify;

namespace SFA.DAS.NotificationService.Infrastructure.UnitTests
{
    [TestFixture]
    public class NotifyEmailServiceTests
    {
        private const string Recipient = "test.user@test.org";
        private Mock<INotifyHttpClientWrapper> _wrapper;
        private NotifyEmailService _emailService;

        [SetUp]
        public void Setup()
        {
            _wrapper = new Mock<INotifyHttpClientWrapper>();
            _emailService = new NotifyEmailService(_wrapper.Object);
        }

        [TearDown]
        public void Teardown()
        {
            _emailService.CloseCircuitBreaker();
        }

        [Test]
        public async Task SuccessfullySentEmail()
        {
            var json = File.ReadAllText(GetFullFilePath("created.json"));

            var email = new EmailMessage
            {
                RecipientsAddress = Recipient,
                Data = new Dictionary<string, string>()
            };

            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Created,
                Content = new StringContent(json)
            };

            _wrapper.Setup(x => x.SendMessage(It.Is<NotifyMessage>(m => m.To == email.RecipientsAddress))).ReturnsAsync(response);

            await _emailService.SendAsync(email);

            Assert.That(_emailService.StatusCode, Is.EqualTo(response.StatusCode));
        }

        [Test]
        public async Task ExceededMaxNumberOfMessages()
        {
            var json = File.ReadAllText(GetFullFilePath("error-response-429.json"));

            var email = new EmailMessage
            {
                RecipientsAddress = Recipient,
                Data = new Dictionary<string, string>()
            };

            var response = new HttpResponseMessage
            {
                StatusCode = (HttpStatusCode)429,
                Content = new StringContent(json)
            };

            _wrapper.Setup(x => x.SendMessage(It.Is<NotifyMessage>(m => m.To == email.RecipientsAddress))).ReturnsAsync(response);

            await _emailService.SendAsync(email);

            Assert.That(_emailService.StatusCode, Is.EqualTo(response.StatusCode));

            await _emailService.SendAsync(email);

            Assert.That(_emailService.StatusCode, Is.EqualTo(HttpStatusCode.ServiceUnavailable));

            _wrapper.Verify(x => x.SendMessage(It.IsAny<NotifyMessage>()), Times.Exactly(1));
        }

        private static string GetFullFilePath(string localFilePath)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files", localFilePath);
        }
    }
}
