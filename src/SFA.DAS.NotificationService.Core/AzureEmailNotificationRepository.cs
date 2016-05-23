using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using SFA.DAS.NotificationService.Application.DataEntities;
using SFA.DAS.NotificationService.Application.Interfaces;
using SFA.DAS.NotificationService.Application.Messages;

namespace SFA.DAS.NotificationService.Application
{
    public class AzureEmailNotificationRepository : IMessageNotificationRepository
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

        public void Create(MessageData message)
        {
            var tableClient = _storageAccount.CreateCloudTableClient();

            var table = tableClient.GetTableReference(TableName);

            var entity = new EmailMessageEntity(message.MessageType, message.MessageId)
            {
                Data = JsonConvert.SerializeObject(message.Data)
            };
            var insertOperation = TableOperation.Insert(entity);

            table.Execute(insertOperation);
        }

        public MessageData Get(string messageType, string messageId)
        {
            var messageData = new MessageData
            {
                MessageType = messageType,
                MessageId = messageId,
                Data = new Dictionary<string, string>()
            };

            var tableClient = _storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference(TableName);

            var tableOperation = TableOperation.Retrieve<EmailMessageEntity>(messageType, messageId);
            var result = table.Execute(tableOperation);

            var configItem = (EmailMessageEntity)result.Result;

            if (configItem != null)
                messageData.Data = JsonConvert.DeserializeObject<Dictionary<string, string>>(configItem.Data);

            return messageData;
        }
    }
}