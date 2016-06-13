using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NSubstitute;
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
        private IMessageNotificationRepository _messageNotificationRepository;
        private IMessageSubSystem _messageSubSystem;
        private SendEmailCommandHandler _commandHandler;

        [SetUp]
        public void Setup()
        {
            _messageNotificationRepository = Substitute.For<IMessageNotificationRepository>();
            _messageSubSystem = Substitute.For<IMessageSubSystem>();

            _commandHandler = new SendEmailCommandHandler(_messageNotificationRepository, new MessagingService(_messageSubSystem));
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

            await _messageNotificationRepository.Received().Create(Arg.Is<MessageData>(x => x.MessageId == messageId.ToString() && x.MessageType == messageType && x.Content.Timestamp == messageTimestamp));

            await _messageSubSystem.Received().EnqueueAsync(Arg.Any<string>());
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