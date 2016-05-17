using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using SFA.DAS.NotificationService.Core.Messages;

namespace SFA.DAS.NotificationService.Core
{
    public class AzureEmailNotificationRepository : IEmailNotificationRepository
    {
        private CloudStorageAccount _storageAccount;

        public AzureEmailNotificationRepository() 
            : this(CloudConfigurationManager.GetSetting("StorageConnectionString"))
        {
        }

        public AzureEmailNotificationRepository(string storageConnectionString)
        {
            _storageAccount = CloudStorageAccount.Parse(storageConnectionString);
        }

        public async Task<string> Create(SendEmailMessage message)
        {
            var messageId = Guid.NewGuid().ToString();

            var tableClient = _storageAccount.CreateCloudTableClient();

            var table = tableClient.GetTableReference("SentEmailMessages");

            var entity = new EmailMessageEntity(message.UserId, messageId)
            {
                Data = JsonConvert.SerializeObject(message)
            };
            var insertOperation = TableOperation.Insert(entity);

            await table.ExecuteAsync(insertOperation);

            return messageId;
        }

        public async Task<SendEmailMessage> Get(string userId, string messageId)
        {
            var tableClient = _storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference("SentEmailMessages");

            var tableOperation = TableOperation.Retrieve<EmailMessageEntity>(userId, messageId);
            var result = await table.ExecuteAsync(tableOperation);

            var configItem = (EmailMessageEntity)result.Result;
            return configItem == null ? null : JsonConvert.DeserializeObject<SendEmailMessage>(configItem.Data);
        }
    }
}