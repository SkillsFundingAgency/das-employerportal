using System;

namespace SFA.DAS.NotificationService.Application.Services
{
    public class DefaultGuidProvider : GuidProvider
    {
        public override Guid NewGuid()
        {
            return Guid.NewGuid();
        }
    }
}