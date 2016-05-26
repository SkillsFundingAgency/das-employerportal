using System;
using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.Messaging;
using SFA.DAS.NotificationService.Application.Commands.SendMessage;
using SFA.DAS.NotificationService.Application.DataEntities;
using SFA.DAS.NotificationService.Application.Interfaces;
using SFA.DAS.NotificationService.Application.Services;
using SFA.DAS.TimeProvider;

namespace SFA.DAS.NotificationService.Application.UnitTests
{
    [TestFixture]
    public class SendMessageCommandHandlerTests
    {
        [TearDown]
        public void Teardown()
        {
            DateTimeProvider.ResetToDefault();
            GuidProvider.ResetToDefault();
        }

        [Test]
        public void Basic()
        {
            var messageNotificationRepository = Substitute.For<IMessageNotificationRepository>();
            var messageSubSystem = Substitute.For<IMessageSubSystem>();

            var commandHandler = new SendMessageCommandHandler(messageNotificationRepository, new MessagingService(messageSubSystem));

            var dateTime = DateTime.UtcNow.AddDays(1);

            DateTimeProvider.Current = new FateTimeProvider(dateTime);

            var guid = Guid.NewGuid();

            GuidProvider.Current = new FakeGuidProvider(guid);

            var messageType = "TestEmail";

            var data = new Dictionary<string, string>
            {
                {"messageType", messageType},
                {"toEmail", "to@test.org"},
                {"fromEmail", "from@test.org"},
                {"subject", "Test Subject"},
                {"body", "Some example text"}
            };

            var cmd = new SendMessageCommand
            {
                Data = data
            };

            var cloneData = data.ToDictionary(entry => entry.Key, entry => entry.Value);

            cloneData.Add("Timestamp", DateTimeProvider.Current.UtcNow.ToString("yyyy-MM-dd HH':'mm':'ss"));

            commandHandler.Handle(cmd);

            var expectedMessageData = new MessageData
            {
                MessageId = GuidProvider.Current.NewGuid().ToString(),
                MessageType = messageType,
                Data = cloneData
            };

            messageNotificationRepository.Received().Create(Arg.Is<MessageData>(x => x.MessageId == expectedMessageData.MessageId && x.MessageType == expectedMessageData.MessageType));

            messageSubSystem.Received().EnqueueAsync(Arg.Any<string>());
        }
    }
}