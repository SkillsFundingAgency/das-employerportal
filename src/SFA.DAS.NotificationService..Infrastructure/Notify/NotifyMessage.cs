using System.Collections.Generic;
using Newtonsoft.Json;

namespace SFA.DAS.NotificationService.Infrastructure.Notify
{
    public class NotifyMessage
    {
        [JsonProperty(PropertyName = "to")]
        public string To { get; set; }
        [JsonProperty(PropertyName = "template")]
        public string Template { get; set; }
        [JsonProperty(PropertyName = "personalisation")]
        public Dictionary<string, string> Personalisation { get; set; }
    }
}