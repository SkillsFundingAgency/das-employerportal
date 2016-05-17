using System;
using System.IO;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.NotificationService.Core.Implementations;
using SFA.DAS.NotificationService.Core.Messages;

namespace SFA.DAS.NotificationService.Core.UnitTests
{
    [TestFixture]
    public class DirectoryQueueReaderTests
    {
        private string _folderName;

        [SetUp]
        public void Setup()
        {
            _folderName = Path.Combine("c:\\temp\\DirectoryQueueReaderTests", Guid.NewGuid().ToString());

            Directory.CreateDirectory(_folderName);
        }

        [TearDown]
        public void Teardown()
        {
            Directory.Delete(_folderName, true);
        }

        [Test]
        public void ReadMessage()
        {
            var reader = new DirectoryQueueReader(_folderName);

            var sentMessage = new SendEmailMessage
            {
                FromEmail = "from@test.org",
                ToEmail = "to@test.org",
                Subject = "Subject",
                Message = "My message"
            };

            CreateFile(sentMessage);

            var message = reader.Read();

            Assert.That(message.FromEmail, Is.EqualTo(sentMessage.FromEmail));
            Assert.That(message.ToEmail, Is.EqualTo(sentMessage.ToEmail));
            Assert.That(message.Subject, Is.EqualTo(sentMessage.Subject));
            Assert.That(message.Message, Is.EqualTo(sentMessage.Message));

            Assert.That(Directory.GetFiles(_folderName).Length, Is.EqualTo(0));
        }

        private string CreateFile(SendEmailMessage message)
        {
            var guid = Guid.NewGuid();

            var filename = Path.Combine(_folderName, $"SendEmailMessage_{guid}.txt");

            var contents = JsonConvert.SerializeObject(message);

            File.WriteAllText(filename, contents);

            return guid.ToString();
        }
    }
}