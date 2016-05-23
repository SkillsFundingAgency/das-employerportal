using System.Collections.Generic;

namespace SFA.DAS.NotificationService.Application.Queries.GetMessage
{
    public class GetMessageQueryResponse
    {
        public string MessageType { get; set; }
        public string MessageId { get; set; }
        public Dictionary<string, string> Data { get; set; }
    }
}