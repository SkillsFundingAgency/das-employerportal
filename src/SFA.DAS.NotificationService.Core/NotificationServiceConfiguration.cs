namespace SFA.DAS.NotificationService.Application
{
    //{"SmtpServer":{"ServerName":"localhost","UserName":"","Password":"","Port":"25"},"ServiceBusConfiguration":{"ConnectionString":"","QueueName":""},"MessageStorage":{"TableName":"SentEmailMessages"},"NotifyEmail":{"ApiBaseUrl":"https://www.notifications.service.gov.uk"}}
    //SendGrid -> <network host="smtp.sendgrid.net" password="password" userName="username" port="587" />
    public class NotificationServiceConfiguration
    {
        public SmtpConfiguration SmtpServer { get; set; }
        public AzureServiceBusMessageSubSystemConfiguration ServiceBusConfiguration { get; set; }
        public MessageStorageConfiguration MessageStorage { get; set; }
        public NotifyEmailServiceConfiguration NotifyEmail { get; set; }

        public PortalJwtTokenConfiguration PortalJwtToken { get; set; }
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

    public class MessageStorageConfiguration
    {
        public string TableName { get; set; }
    }

    public class NotifyEmailServiceConfiguration
    {
        public string ApiBaseUrl { get; set; }
        public string ServiceId { get; set; }
        public string ApiKey { get; set; }
        public string EmailTemplateId { get; set; }
    }

    public class PortalJwtTokenConfiguration
    {
        public string AudienceId { get; set; }
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public bool AllowInsecureHttp { get; set; }
        public int AccessTokenExpiryTimeInMinutes { get; set; }
    }

}