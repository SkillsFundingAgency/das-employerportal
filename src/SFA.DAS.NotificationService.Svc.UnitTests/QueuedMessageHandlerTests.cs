using System.Threading.Tasks;
using MediatR;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.Messaging;
using SFA.DAS.NotificationService.Application.Interfaces;

namespace SFA.DAS.NotificationService.Worker.UnitTests
{
    [TestFixture]
    public class QueuedMessageHandlerTests
    {
        private IMessageSubSystem _messageSubSsytem;
        private IMediator _mediator;
        private IEmailService _emailService;
        private MessagingService _messagingService;

        [SetUp]
        public void Setup()
        {
            _messageSubSsytem = Substitute.For<IMessageSubSystem>();
            _mediator = Substitute.For<IMediator>();
            _emailService = Substitute.For<IEmailService>();
            _messagingService = new MessagingService(_messageSubSsytem);
        }

        [Test]
        public async Task NullContentWillNotBeProcessed()
        {
            _messageSubSsytem.DequeueAsync().Returns((SubSystemMessage)null);


        }
    }
}