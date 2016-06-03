using SFA.DAS.NotificationService.Application.DataEntities;

namespace SFA.DAS.NotificationService.Application.Queries.GetMessage
{
    public class GetMessageQueryResponse
    {
        public string MessageType { get; set; }
        public string MessageId { get; set; }
        public MessageContent Content { get; set; }
    }
}