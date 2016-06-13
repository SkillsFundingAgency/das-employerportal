using System;
using SFA.DAS.TimeProvider;

namespace SFA.DAS.NotificationService.Application.UnitTests
{
    public class FakeTimeProvider : DateTimeProvider
    {
        private readonly DateTime _dateTime;

        public FakeTimeProvider(DateTime dateTime)
        {
            _dateTime = dateTime;
        }

        public override DateTime UtcNow => _dateTime;
    }
}