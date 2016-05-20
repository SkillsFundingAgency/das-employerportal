using System;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using SFA.DAS.NotificationService.Application.DataEntities;
using SFA.DAS.NotificationService.Application.Interfaces;
using SFA.DAS.NotificationService.Application.Messages;

namespace SFA.DAS.NotificationService.Application
{
    public class AzureEmailNotificationRepository : IEmailNotificationRepository
    {
        private const string TableName = "SentEmailMessages";

        private readonly CloudStorageAccount _storageAccount;

        public AzureEmailNotificationRepository() 
            : this(CloudConfigurationManager.GetSetting("StorageConnectionString"))
        {
        }

        public AzureEmailNotificationRepository(string storageConnectionString)
        {
            _storageAccount = CloudStorageAccount.Parse(storageConnectionString);
        }

        public string Create(SendEmailData message)
        {
            var messageId = Guid.NewGuid().ToString();

            var tableClient = _storageAccount.CreateCloudTableClient();

            var table = tableClient.GetTableReference(TableName);

            var entity = new EmailMessageEntity(message.UserId, messageId)
            {
                Data = JsonConvert.SerializeObject(message)
            };
            var insertOperation = TableOperation.Insert(entity);

            table.Execute(insertOperation);

            return messageId;
        }

        public SendEmailMessage Get(string userId, string messageId)
        {
            var tableClient = _storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference(TableName);

            var tableOperation = TableOperation.Retrieve<EmailMessageEntity>(userId, messageId);
            var result = table.Execute(tableOperation);

            var configItem = (EmailMessageEntity)result.Result;
            return configItem == null ? null : JsonConvert.DeserializeObject<SendEmailMessage>(configItem.Data);
        }
    }
}