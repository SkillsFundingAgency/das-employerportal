namespace SFA.DAS.NotificationService.Application
{
    //{"SmtpServer":{"ServerName":"localhost","UserName":"","Password":"","Port":""},"ServiceBusConfiguration":{"ConnectionString":"test","QueueName":"MyQueue"}}
    //SendGrid -> <network host="smtp.sendgrid.net" password="password" userName="username" port="587" />
    public class NotificationServiceConfiguration
    {
        public SmtpConfiguration SmtpServer { get; set; }
        public AzureServiceBusMessageSubSystemConfiguration ServiceBusConfiguration { get; set; }
    }

    public class SmtpConfiguration
    {
        public string ServerName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Port { get; set; }
    }

    public class AzureServiceBusMessageSubSystemConfiguration
    {
        public string ConnectionString { get; set; }
        public string QueueName { get; set; }
    }
}