using Microsoft.AspNetCore.Mvc;
using NotificationService.Application.Abstractions.Messaging;
using NotificationService.Application.Notifications.Commands;

namespace NotificationService.Api.Controllers
{
    [ApiController]
    [Route("api/test")]
    public class TestController : ControllerBase
    {
        private readonly IMessageBus _messageBus;

        public TestController(IMessageBus messageBus)
        {
            _messageBus = messageBus;
        }

        [HttpPost]
        public async Task<ActionResult> SendTestMessageToRabbit([FromBody] SendNotificationMessage message, CancellationToken cancellationToken)
        {
            await _messageBus.PublishAsync(message);

            return Ok();
        }
    }
}
