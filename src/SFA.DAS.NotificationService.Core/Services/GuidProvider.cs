using System;

namespace SFA.DAS.NotificationService.Application.Services
{
    public abstract class GuidProvider
    {
        private static GuidProvider current;

        static GuidProvider()
        {
            current = new DefaultGuidProvider();
        }

        public static GuidProvider Current
        {
            get { return current; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value), "You must provide a valid TimeProvider");

                current = value;
            }
        }

        public abstract Guid NewGuid();

        public static void ResetToDefault()
        {
            current = new DefaultGuidProvider();
        }

    }
}