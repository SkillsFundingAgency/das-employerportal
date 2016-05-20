using Microsoft.WindowsAzure.Storage.Table;

namespace SFA.DAS.NotificationService.Application
{
    public class EmailMessageEntity : TableEntity
    {
        public EmailMessageEntity(string userId, string messageId)
            : base(userId, messageId)
        {
        }

        public EmailMessageEntity() {}

        public string Data { get; set; }    
    }
}