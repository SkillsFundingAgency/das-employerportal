//using NLog;
//using SFA.DAS.Configuration;
//using SFA.DAS.EmployerUsers.WebClientComponents;

//namespace SFA.DAS.EmployerPortal.Infrastructure.Configuration
//{
//    public class EmployerPortalClientComponentConfigurationFactory : ConfigurationFactory
//    {
//        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

//        private readonly IConfigurationService _configurationService;

//        public EmployerPortalClientComponentConfigurationFactory(IConfigurationService configurationService)
//        {
//            _configurationService = configurationService;
//        }

//        public override ConfigurationContext Get()
//        {
//            _logger.Debug("Start reading configuration");

//            var usersConfig = _configurationService.Get<EmployerPortalConfiguration>();

//            _logger.Debug($"Got configuration. BaseUrl = {usersConfig.IdentifyingParty.ApplicationBaseUrl}");

//            return new ConfigurationContext
//            {
//                AccountActivationUrl = usersConfig.IdentifyingParty.ApplicationBaseUrl + "account/confirm/"
//            };
//        }
//    }
//}
