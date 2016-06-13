using MediatR;

namespace SFA.DAS.NotificationService.Application.Queries.GetMessage
{
    public class GetMessageQueryRequest : IRequest<GetMessageQueryResponse>
    {
        public string MessageType { get; set; }
        public string MessageId { get; set; }
    }
}