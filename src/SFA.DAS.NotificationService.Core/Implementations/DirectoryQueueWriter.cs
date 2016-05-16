using System;
using System.IO;
using Newtonsoft.Json;
using SFA.DAS.NotificationService.Core.Interfaces;
using SFA.DAS.NotificationService.Core.Messages;

namespace SFA.DAS.NotificationService.Core.Implementations
{
    public class DirectoryQueueWriter : IQueueWriter
    {
        private readonly string _path;

        public DirectoryQueueWriter(string path)
        {
            _path = path;
        }

        public void Write(SendEmailMessage message)
        {
            var filename = Path.Combine(_path, $"SendEmailMessage_{Guid.NewGuid()}.txt");

            var contents = JsonConvert.SerializeObject(message);

            File.WriteAllText(filename, contents);
        }
    }
}