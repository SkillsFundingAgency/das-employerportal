using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.NotificationService.Application.DataEntities;
using SFA.DAS.NotificationService.Application.Interfaces;
using SFA.DAS.NotificationService.Application.Messages;
using SFA.DAS.NotificationService.Application.Queries.GetMessage;
using SFA.DAS.NotificationService.Application.Services;
using SFA.DAS.TimeProvider;

namespace SFA.DAS.NotificationService.Application.UnitTests.QueryTests
{
    [TestFixture]
    public class GetMessageQueryHandlerTests
    {
        private Mock<IMessageNotificationRepository> _messageNotificationRepository;
        private GetMessageQueryHandler _handler;

        [SetUp]
        public void Setup()
        {
            _messageNotificationRepository = new Mock<IMessageNotificationRepository>();
            _handler = new GetMessageQueryHandler(_messageNotificationRepository.Object);
        }

        [Test]
        public async Task ReturnsEmailMessage()
        {
            var userId = GuidProvider.Current.NewGuid().ToString();

            var request = new GetMessageQueryRequest
            {
                MessageType = "SendEmail",
                MessageId = GuidProvider.Current.NewGuid().ToString()
            };

            var emailContent = new EmailContent
            {
                RecipientsAddress = "to@test.org",
                ReplyToAddress = "from@test.org",
                Data = new Dictionary<string, string>
                {
                    {"Item1", "Data1"},
                    {"Item2", "Data2"},
                    {"Item3", "Data3"}
                }
            };

            _messageNotificationRepository.Setup(x => x.Get(request.MessageType, request.MessageId)).ReturnsAsync(new MessageData
            {
                MessageType = request.MessageType,
                MessageId = request.MessageId,
                Content = new MessageContent
                {
                    MessageFormat = MessageFormat.Email,
                    ForceFormat = true,
                    UserId = userId,
                    Timestamp = DateTimeProvider.Current.UtcNow,
                    Data = JsonConvert.SerializeObject(emailContent)
                }
            });

            var response = await _handler.Handle(request);

            Assert.That(response.Content, Is.Not.Null);
            Assert.That(response.Content.UserId, Is.EqualTo(userId));

            var fetctedEmail = JsonConvert.DeserializeObject<EmailContent>(response.Content.Data);

            Assert.That(fetctedEmail.RecipientsAddress, Is.EqualTo(emailContent.RecipientsAddress));
            Assert.That(fetctedEmail.ReplyToAddress, Is.EqualTo(emailContent.ReplyToAddress));
        }
    }
}