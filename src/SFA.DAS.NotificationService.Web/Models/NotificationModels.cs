namespace SFA.DAS.NotificationService.Web.Models
{
    public class EmailNotification
    {
        public string ToEmail { get; set; }
        public string FromEmail { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}