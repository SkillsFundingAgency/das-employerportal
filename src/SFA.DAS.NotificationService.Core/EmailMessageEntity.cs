using Microsoft.WindowsAzure.Storage.Table;

namespace SFA.DAS.NotificationService.Application
{
    public class EmailMessageEntity : TableEntity
    {
        public EmailMessageEntity(string userId, string messageId)
        {
            PartitionKey = userId;
            RowKey = messageId;
        }

        public string Data { get; set; }    
    }
}