using System;
using SFA.DAS.NotificationService.Application.Services;

namespace SFA.DAS.NotificationService.Application.UnitTests.TestHelpers
{
    public class FakeGuidProvider : GuidProvider
    {
        private readonly Guid _guid;

        public FakeGuidProvider(Guid guid)
        {
            _guid = guid;
        }

        public override Guid NewGuid()
        {
            return _guid;
        }
    }
}