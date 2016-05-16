using System;
using System.IO;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.NotificationService.Core.Implementations;
using SFA.DAS.NotificationService.Core.Messages;

namespace SFA.DAS.NotificationService.Core.UnitTests
{
    [TestFixture]
    public class DirectoryQueueWriterTests
    {
        private string _folderName;

        [SetUp]
        public void Setup()
        {
            _folderName = Path.Combine("c:\\temp", Guid.NewGuid().ToString());

            Directory.CreateDirectory(_folderName);
        }

        [TearDown]
        public void Teardown()
        {
            Directory.Delete(_folderName, true);
        }

        [Test]
        public void CreateMessage()
        {
            var writer = new DirectoryQueueWriter(_folderName);

            var sentMessage = new SendEmailMessage
            {
                FromEmail = "from@test.org",
                ToEmail = "to@test.org",
                Subject = "Subject",
                Message = "My message"
            };

            writer.Write(sentMessage);

            var files = Directory.GetFiles(_folderName);

            Assert.That(files.Length, Is.EqualTo(1));
            
            var fileinfo = new FileInfo(files[0]);

            Assert.That(fileinfo.Name.StartsWith("SendEmailMessage_"));

            var message = JsonConvert.DeserializeObject<SendEmailMessage>(File.ReadAllText(files[0]));

            Assert.That(message.FromEmail, Is.EqualTo(sentMessage.FromEmail));
            Assert.That(message.ToEmail, Is.EqualTo(sentMessage.ToEmail));
            Assert.That(message.Subject, Is.EqualTo(sentMessage.Subject));
            Assert.That(message.Message, Is.EqualTo(sentMessage.Message));
        }
    }
}
