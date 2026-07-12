using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Api.Contracts;
using NotificationService.Application.Abstractions.DataAccess;
using NotificationService.Application.Abstractions.Messaging;
using NotificationService.Application.Notifications.Commands;
using NotificationService.Core.Models;
using NotificationService.Core.Primitives.Enums;
using NotificationService.Core.ValueObjects;

namespace NotificationService.Api.Controllers
{
    [ApiController]
    [Route("api/test")]
    public class TestController : ControllerBase
    {
        private readonly IMessageBus _messageBus;
        private readonly IBaseRepository<Notification> _repository;

        public TestController(IMessageBus messageBus, IBaseRepository<Notification> repository)
        {
            _messageBus = messageBus;
            _repository = repository;
        }

        [HttpPost]
        public async Task<ActionResult> SendTestMessageToRabbit([FromBody] SendNotificationMessage message, CancellationToken cancellationToken)
        {
            await _messageBus.PublishAsync(message);

            return Ok();
        }

        [HttpPost("/create")]
        public async Task<ActionResult> CreateMessageAsync([FromBody] NotificationRequest notificationRequest, CancellationToken cancellationToken)
        {
            var notificationResult =
                Recipient.Create(
                    notificationRequest.UserId,
                    notificationRequest.Provider,
                    notificationRequest.RecipientValue)
                .Bind(recipient =>
                    Core.ValueObjects.Content.Create(
                        notificationRequest.ContentType,
                        notificationRequest.Subject,
                        notificationRequest.ContentValue)
                    .Bind(content =>
                        Notification.Create(
                            Guid.NewGuid(),
                            Status.Processing,
                            recipient,
                            content)));

            await _repository.AddAsync(notificationResult.Value, cancellationToken);

            return Ok(notificationResult.Value.Id);
        }

        [HttpGet]
        public async Task<ActionResult> GetMessagesAsync(CancellationToken cancellationToken)
        {
            var notifications = await _repository.GetAllAsync(cancellationToken);

            return Ok(notifications);
        }
    }
}
