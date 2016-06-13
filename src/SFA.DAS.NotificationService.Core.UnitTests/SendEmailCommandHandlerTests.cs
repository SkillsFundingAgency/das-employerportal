using System;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.Messaging;
using SFA.DAS.NotificationService.Application.Commands.SendEmail;
using SFA.DAS.NotificationService.Application.DataEntities;
using SFA.DAS.NotificationService.Application.Interfaces;
using SFA.DAS.NotificationService.Application.Services;
using SFA.DAS.TimeProvider;

namespace SFA.DAS.NotificationService.Application.UnitTests
{
    [TestFixture]
    public class SendEmailCommandHandlerTests
    {
        [TearDown]
        public void Teardown()
        {
            DateTimeProvider.ResetToDefault();
            GuidProvider.ResetToDefault();
        }

        [Test]
        public void HandlesEmailMessage()
        {
            var messageNotificationRepository = Substitute.For<IMessageNotificationRepository>();
            var messageSubSystem = Substitute.For<IMessageSubSystem>();

            var commandHandler = new SendEmailCommandHandler(messageNotificationRepository, new MessagingService(messageSubSystem));

            var dateTime = DateTime.UtcNow.AddDays(1);

            DateTimeProvider.Current = new FateTimeProvider(dateTime);

            var guid = Guid.NewGuid();

            GuidProvider.Current = new FakeGuidProvider(guid);

            var messageType = "TestEmail";

            var cmd = new SendEmailCommand
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

            commandHandler.Handle(cmd);

            messageNotificationRepository.Received().Create(Arg.Is<MessageData>(x => x.MessageType == messageType));

            messageSubSystem.Received().EnqueueAsync(Arg.Any<string>());
        }
    }
}