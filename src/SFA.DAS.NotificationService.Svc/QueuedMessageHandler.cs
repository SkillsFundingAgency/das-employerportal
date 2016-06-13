﻿using System;
using MediatR;
using Newtonsoft.Json;
using SFA.DAS.Messaging;
using SFA.DAS.NotificationService.Application.DataEntities;
using SFA.DAS.NotificationService.Application.Interfaces;
using SFA.DAS.NotificationService.Application.Messages;
using SFA.DAS.NotificationService.Application.Queries.GetMessage;

namespace SFA.DAS.NotificationService.Worker
{
    public class QueuedMessageHandler
    {
        private readonly IMediator _mediator;
        private readonly MessagingService _messagingService;
        private readonly IEmailService _emailService;

        public QueuedMessageHandler(IMediator mediator, MessagingService messagingService, IEmailService emailService)
        {
            if (mediator == null)
                throw new ArgumentNullException(nameof(mediator));
            if (messagingService == null)
                throw new ArgumentNullException(nameof(messagingService));
            if (emailService == null)
                throw new ArgumentNullException(nameof(emailService));
            _mediator = mediator;
            _messagingService = messagingService;
            _emailService = emailService;
        }

        public void Handle()
        {
            var message = _messagingService.ReceiveAsync<QueueMessage>().Result;

            if (message.Content != null)
            {
                var savedMessage = _mediator.Send(new GetMessageQueryRequest
                {
                    MessageType = message.Content.MessageType,
                    MessageId = message.Content.MessageId
                });

                if (savedMessage.Content != null)
                {
                    var messageFormat = savedMessage.Content.MessageFormat;
                    if (messageFormat == MessageFormat.Email)
                    {
                        var emailContent = JsonConvert.DeserializeObject<EmailContent>(savedMessage.Content.Data);

                        _emailService.Send(new EmailMessage
                        {
                            MessageType = savedMessage.MessageType,
                            UserId = savedMessage.Content.UserId,
                            RecipientsAddress = emailContent.RecipientsAddress,
                            ReplyToAddress = emailContent.ReplyToAddress,
                            Data = emailContent.Data
                        });
                    }
                }

                message.CompleteAsync().Wait();
            }

        }
    }
}