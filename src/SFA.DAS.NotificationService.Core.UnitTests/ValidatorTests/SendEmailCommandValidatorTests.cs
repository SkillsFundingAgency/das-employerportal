﻿using System.Linq;
using NUnit.Framework;
using SFA.DAS.NotificationService.Application.Commands.SendEmail;
using SFA.DAS.NotificationService.Application.Services;

namespace SFA.DAS.NotificationService.Application.UnitTests.ValidatorTests
{
    [TestFixture]
    public class SendEmailCommandValidatorTests
    {
        private SendEmailCommandValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new SendEmailCommandValidator();
        }

        [Test]
        public void ThenSuceedsWithValidCommand()
        {
            var cmd = BuildValidCommand();

            var result = _validator.Validate(cmd);

            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Errors.Count, Is.EqualTo(0));
        }

        [Test]
        public void ThenFailsAllItems()
        {
            var cmd = new SendEmailCommand();

            var result = _validator.Validate(cmd);

            Assert.That(result.IsValid, Is.False);
            Assert.That(result.Errors.Count, Is.EqualTo(4));
            Assert.That(result.Errors.FirstOrDefault(x => x.PropertyName == "MessageType"), Is.Not.Null);
            Assert.That(result.Errors.FirstOrDefault(x => x.PropertyName == "UserId"), Is.Not.Null);
            Assert.That(result.Errors.FirstOrDefault(x => x.PropertyName == "RecipientsAddress"), Is.Not.Null);
            Assert.That(result.Errors.FirstOrDefault(x => x.PropertyName == "ReplyToAddress"), Is.Not.Null);
        }

        [Test]
        public void ThenFailsWhenMessageTypeIsEmpty()
        {
            var cmd = BuildValidCommand();

            cmd.MessageType = "";

            var result = _validator.Validate(cmd);

            Assert.That(result.IsValid, Is.False);
            Assert.That(result.Errors.Count, Is.EqualTo(1));
            Assert.That(result.Errors[0].PropertyName, Is.EqualTo("MessageType"));
        }

        [Test]
        public void ThenFailsWhenUserIdIsEmpty()
        {
            var cmd = BuildValidCommand();

            cmd.UserId = "";

            var result = _validator.Validate(cmd);

            Assert.That(result.IsValid, Is.False);
            Assert.That(result.Errors.Count, Is.EqualTo(1));
            Assert.That(result.Errors[0].PropertyName, Is.EqualTo("UserId"));
        }

        [Test]
        public void ThenFailsWhenRecipientEmailIsEmpty()
        {
            var cmd = BuildValidCommand();

            cmd.RecipientsAddress = "";

            var result = _validator.Validate(cmd);

            Assert.That(result.IsValid, Is.False);
            Assert.That(result.Errors.Count, Is.EqualTo(1));
            Assert.That(result.Errors[0].PropertyName, Is.EqualTo("RecipientsAddress"));
        }

        [Test]
        public void ThenFailsWhenReplyToEmailIsEmpty()
        {
            var cmd = BuildValidCommand();

            cmd.ReplyToAddress = "";

            var result = _validator.Validate(cmd);

            Assert.That(result.IsValid, Is.False);
            Assert.That(result.Errors.Count, Is.EqualTo(1));
            Assert.That(result.Errors[0].PropertyName, Is.EqualTo("ReplyToAddress"));
        }

        private static SendEmailCommand BuildValidCommand()
        {
            return new SendEmailCommand
            {
                MessageType = "Test Message",
                UserId = GuidProvider.Current.NewGuid().ToString(),
                RecipientsAddress = "recipient@test.org",
                ReplyToAddress = "replyto@test.org"
            };
        }
    }
}