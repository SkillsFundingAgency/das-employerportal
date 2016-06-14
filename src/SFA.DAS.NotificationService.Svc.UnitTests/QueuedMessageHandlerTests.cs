using System;
using System.Threading.Tasks;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.Messaging;
using SFA.DAS.NotificationService.Application.Interfaces;
using SFA.DAS.NotificationService.Application.Messages;
using SFA.DAS.NotificationService.Application.Queries.GetMessage;

namespace SFA.DAS.NotificationService.Worker.UnitTests
{
    [TestFixture]
    public class QueuedMessageHandlerTests
    {
        private Mock<IMessageSubSystem> _messageSubSystem;
        private Mock<IMediator> _mediator;
        private Mock<IEmailService> _emailService;
        private MessagingService _messagingService;
        private QueuedMessageHandler _queuedMessageHandler;

        [SetUp]
        public void Setup()
        {
            _messageSubSystem = new Mock<IMessageSubSystem>();
            _mediator = new Mock<IMediator>();
            _emailService = new Mock<IEmailService>();
            _messagingService = new MessagingService(_messageSubSystem.Object);

            _queuedMessageHandler = new QueuedMessageHandler(_mediator.Object, _messagingService, _emailService.Object);
        }

        [Test]
        public async Task NullContentWillNotBeProcessed()
        {
            _messageSubSystem.Setup(x => x.DequeueAsync()).ReturnsAsync((SubSystemMessage)null);

            await _queuedMessageHandler.Handle();

            _mediator.Verify(x => x.SendAsync(It.IsAny<GetMessageQueryRequest>()), Times.Never);
        }

        [Test]
        public async Task MessageWithContentWillBeProcessed()
        {
            var queueMessage = new QueueMessage
            {
                MessageType = "TestMessage",
                MessageId = Guid.NewGuid().ToString()
            };

            _messageSubSystem.Setup(x => x.DequeueAsync()).ReturnsAsync(new FakeSubSystemMessage(queueMessage));

            await _queuedMessageHandler.Handle();

            _mediator.Verify(med => med.SendAsync(It.Is<GetMessageQueryRequest>(x => x.MessageId == queueMessage.MessageId && x.MessageType == queueMessage.MessageType)));
        }
    }
}