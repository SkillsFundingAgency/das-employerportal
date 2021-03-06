﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.NotificationService.Api.Models;
using SFA.DAS.NotificationService.Api.Orchestrators;
using SFA.DAS.NotificationService.Application.Commands.SendEmail;

namespace SFA.DAS.NotificationService.Api.UnitTests.OrchestratorTests
{
    [TestFixture]
    public class NotificationOrchestratorTests
    {
        private Mock<IMediator> _mediator;

        [SetUp]
        public void Setup()
        {
            _mediator = new Mock<IMediator>();
        }

        [Test]
        public async Task ReturnsExpectedResponseCodeIfEmailSent()
        {
            var emailViewModel = BuildEmailViewModel();

            var orchestrator = new NotificationOrchestrator(_mediator.Object);

            var response = await orchestrator.SendEmail(emailViewModel);

            Assert.That(response.Code, Is.EqualTo(NotificationOrchestratorCodes.Post.Success));
        }

        [Test]
        public async Task CallsExpectedMediatorMethod()
        {
            var emailViewModel = BuildEmailViewModel();

            var orchestrator = new NotificationOrchestrator(_mediator.Object);

            await orchestrator.SendEmail(emailViewModel);

            _mediator.Verify(med => med.SendAsync(It.Is<SendEmailCommand>(x => x.UserId == emailViewModel.UserId
                    && x.MessageType == emailViewModel.MessageType && x.RecipientsAddress == emailViewModel.RecipientsAddress
                    && x.ReplyToAddress == emailViewModel.ReplyToAddress && x.ForceFormat == emailViewModel.ForceFormat
                    && x.Data == emailViewModel.Data)));
        }

        [Test]
        public async Task ReturnsExpectedResponseCodeOnValidationfailure()
        {
            var emailViewModel = BuildEmailViewModel();

            emailViewModel.UserId = string.Empty;

            var orchestrator = new NotificationOrchestrator(_mediator.Object);

            var response = await orchestrator.SendEmail(emailViewModel);

            Assert.That(response.Code, Is.EqualTo(NotificationOrchestratorCodes.Post.ValidationFailure));
        }

        private EmailViewModel BuildEmailViewModel()
        {
            return new EmailViewModel
            {
                UserId = Guid.NewGuid().ToString(),
                MessageType = "TestMessage",
                RecipientsAddress = "recipient@test.org",
                ReplyToAddress = "replyto@test.org",
                ForceFormat = false,
                Data = new Dictionary<string, string>
                {
                    {"Item1", "Value1" },
                    {"Item2", "Value2" }
                }
            };
        }
    }
}