using System;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.NotificationService.Application.Interfaces;

namespace SFA.DAS.NotificationService.Application.Queries.GetMessage
{
    public class GetMessageQueryHandler : IAsyncRequestHandler<GetMessageQueryRequest, GetMessageQueryResponse>
    {
        private readonly IMessageNotificationRepository _messageRepository;

        public GetMessageQueryHandler(IMessageNotificationRepository messageRepository)
        {
            if (messageRepository == null)
                throw new ArgumentNullException(nameof(messageRepository));
            _messageRepository = messageRepository;
        }

        public async Task<GetMessageQueryResponse> Handle(GetMessageQueryRequest message)
        {
            var storedMessage = await _messageRepository.Get(message.MessageType, message.MessageId);

            return new GetMessageQueryResponse
            {
                MessageType = storedMessage.MessageType,
                MessageId = storedMessage.MessageId,
                Content = storedMessage.Content
            };
        }
    }
}