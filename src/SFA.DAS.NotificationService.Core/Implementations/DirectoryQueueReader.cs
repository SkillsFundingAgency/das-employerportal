using System.IO;
using System.Linq;
using Newtonsoft.Json;
using SFA.DAS.NotificationService.Core.Interfaces;
using SFA.DAS.NotificationService.Core.Messages;

namespace SFA.DAS.NotificationService.Core.Implementations
{
    public class DirectoryQueueReader : IQueueReader
    {
        private readonly string _path;

        public DirectoryQueueReader(string path)
        {
            _path = path;
        }

        public SendEmailMessage Read()
        {
            var fileInfo = new DirectoryInfo(_path).GetFileSystemInfos()
                                .OrderBy(fi => fi.CreationTime).First();

            var message = JsonConvert.DeserializeObject<SendEmailMessage>(File.ReadAllText(fileInfo.FullName));

            File.Delete(fileInfo.FullName);

            return message;
        }
    }
}