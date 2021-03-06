﻿using System;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using NLog;
using SFA.DAS.Messaging;
using SFA.DAS.NotificationService.Application.Interfaces;
using SFA.DAS.NotificationService.Application.Messages;
using SFA.DAS.NotificationService.Application.Queries.GetMessage;

namespace SFA.DAS.NotificationService.Worker
{
    public class QueuedMessageHandler
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

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

        public async Task Handle()
        {
            var message = await _messagingService.ReceiveAsync<QueueMessage>();

            if (message.Content != null)
            {
                Logger.Info($"Received message {message.Content.MessageId}");

                try
                {
                    var savedMessage = await _mediator.SendAsync(new GetMessageQueryRequest
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

                            await _emailService.SendAsync(new EmailMessage
                            {
                                MessageType = savedMessage.MessageType,
                                TemplateId = savedMessage.Content.TemplateId,
                                UserId = savedMessage.Content.UserId,
                                RecipientsAddress = emailContent.RecipientsAddress,
                                ReplyToAddress = emailContent.ReplyToAddress,
                                Data = emailContent.Data
                            });
                        }
                    }
                    
                    await message.CompleteAsync();
                    Logger.Info($"Finished processing message {message.Content.MessageId}");
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, $"Error processing message {message.Content.MessageId} - {ex.Message}");
                }

            }
        }
    }
}