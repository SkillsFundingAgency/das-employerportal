using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using SFA.DAS.Configuration;
using SFA.DAS.NotificationService.Application.DataEntities;
using SFA.DAS.NotificationService.Application.Interfaces;

namespace SFA.DAS.NotificationService.Application
{
    public class AzureEmailNotificationRepository : IMessageNotificationRepository
    {
        private readonly IConfigurationService _configurationService;
        private const string DefaultTableName = "SentEmailMessages";

        private readonly CloudStorageAccount _storageAccount;

        public AzureEmailNotificationRepository(IConfigurationService configurationService) 
            : this(configurationService, CloudConfigurationManager.GetSetting("StorageConnectionString"))
        {
        }

        public AzureEmailNotificationRepository(IConfigurationService configurationService, string storageConnectionString)
        {
            if (configurationService == null)
                throw new ArgumentNullException(nameof(configurationService));
            _configurationService = configurationService;
            _storageAccount = CloudStorageAccount.Parse(storageConnectionString);
        }

        public void Create(MessageData message)
        {
            var tableClient = _storageAccount.CreateCloudTableClient();

            var table = tableClient.GetTableReference(GetTableName());

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
            var table = tableClient.GetTableReference(GetTableName());

            var tableOperation = TableOperation.Retrieve<EmailMessageEntity>(messageType, messageId);
            var result = table.Execute(tableOperation);

            var configItem = (EmailMessageEntity)result.Result;

            if (configItem != null)
                messageData.Data = JsonConvert.DeserializeObject<Dictionary<string, string>>(configItem.Data);

            return messageData;
        }

        private string GetTableName()
        {
            var configuration = _configurationService.Get<NotificationServiceConfiguration>();

            if (!string.IsNullOrEmpty(configuration.MessageStorage?.TableName))
            {
                return configuration.MessageStorage.TableName;
            }

            return DefaultTableName;
        }
    }
}