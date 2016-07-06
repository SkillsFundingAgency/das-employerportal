using Newtonsoft.Json;

namespace SFA.DAS.NotificationService.Infrastructure.Notify
{
    public class NotifyResponse
    {
        public NotifyResponseData Data { get; set; }
    }

    public class NotifyResponseData
    {
        public NotifyResponseDataKey Notification { get; set; }

        public string Body { get; set; }

        [JsonProperty(PropertyName = "template_version")]
        public int TemplateVersion { get; set; }

        public string Subject { get; set; }
    }

    public class NotifyResponseDataKey
    {
        public int Id { get; set; }
    }
}