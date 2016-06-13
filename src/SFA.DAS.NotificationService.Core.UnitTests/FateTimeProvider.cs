using System;
using SFA.DAS.TimeProvider;

namespace SFA.DAS.NotificationService.Application.UnitTests
{
    public class FateTimeProvider : DateTimeProvider
    {
        private readonly DateTime _dateTime;

        public FateTimeProvider(DateTime dateTime)
        {
            _dateTime = dateTime;
        }

        public override DateTime UtcNow => _dateTime;
    }
}