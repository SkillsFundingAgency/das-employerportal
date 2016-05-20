using System.Collections.Generic;

namespace SFA.DAS.NotificationService.Web.Models
{
    public class EmailNotification
    {
        public string UserId { get; set; }
        public List<KeyValuePair<string, string>> Data { get; set; }
    }
}