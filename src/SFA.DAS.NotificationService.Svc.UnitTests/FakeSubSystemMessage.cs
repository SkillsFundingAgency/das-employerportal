using Newtonsoft.Json;
using SFA.DAS.Messaging;
using SFA.DAS.NotificationService.Application.Messages;

namespace SFA.DAS.NotificationService.Worker.UnitTests
{
    public class FakeSubSystemMessage : SubSystemMessage
    {
        public FakeSubSystemMessage(QueueMessage content)
        {
            base.Content = JsonConvert.SerializeObject(content);
        }
    }
}