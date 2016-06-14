using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Messaging;
using SFA.DAS.NotificationService.Application.Commands.SendEmail;
using SFA.DAS.NotificationService.Application.DataEntities;
using SFA.DAS.NotificationService.Application.Exceptions;
using SFA.DAS.NotificationService.Application.Interfaces;
using SFA.DAS.NotificationService.Application.Services;
using SFA.DAS.TimeProvider;

namespace SFA.DAS.NotificationService.Application.UnitTests.CommandTests
{
    [TestFixture]
    public class SendEmailCommandHandlerTests
    {
        private Mock<IMessageNotificationRepository> _messageNotificationRepository;
        private Mock<IMessageSubSystem> _messageSubSystem;
        private SendEmailCommandHandler _commandHandler;

        [SetUp]
        public void Setup()
        {
            _messageNotificationRepository = new Mock<IMessageNotificationRepository>();
            _messageSubSystem = new Mock<IMessageSubSystem>();

            _commandHandler = new SendEmailCommandHandler(_messageNotificationRepository.Object, new MessagingService(_messageSubSystem.Object));
        }

        [TearDown]
        public void Teardown()
        {
            DateTimeProvider.ResetToDefault();
            GuidProvider.ResetToDefault();
        }

        [Test]
        public async Task ThenMessageIsStoredAndSent()
        {
            var messageTimestamp = DateTime.UtcNow.AddDays(1);
            var messageId = Guid.NewGuid();

            DateTimeProvider.Current = new FakeTimeProvider(messageTimestamp);
            GuidProvider.Current = new FakeGuidProvider(messageId);

            const string messageType = "TestEmail";

            var cmd = BuildSendEmailCommand(messageType);

            await _commandHandler.Handle(cmd);

            _messageNotificationRepository.Verify(repo => repo.Create(It.Is<MessageData>(x => x.MessageId == messageId.ToString() && x.MessageType == messageType && x.Content.Timestamp == messageTimestamp)));

            _messageSubSystem.Verify(x => x.EnqueueAsync(It.IsAny<string>()));
        }

        [Test]
        public void ThenInvalidCommandFailsValidation()
        {
            var cmd = new SendEmailCommand();

            var expectedException = Assert.ThrowsAsync<CustomValidationException>(async () => await _commandHandler.Handle(cmd));

            Assert.That(expectedException.ValidationResult.IsValid, Is.False);
        }

        private static SendEmailCommand BuildSendEmailCommand(string messageType)
        {
            return new SendEmailCommand
            {
                UserId = "User1",
                MessageType = messageType,
                RecipientsAddress = "to@test.org",
                ReplyToAddress = "from@test.org",
                ForceFormat = false,
                Data = new Dictionary<string, string>
                {
                    {"subject", "Test Subject"},
                    {"body", "Some example text"}
                }
            };
        }
    }
}