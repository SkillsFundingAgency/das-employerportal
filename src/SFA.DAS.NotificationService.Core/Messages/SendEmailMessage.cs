namespace SFA.DAS.NotificationService.Core.Messages
{
    public class SendEmailMessage
    {
        public string ToEmail { get; set; }
        public string FromEmail { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}