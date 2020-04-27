using System.Threading;
using System.Threading.Tasks;
using CoronaAppZim.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoronaAppZim.Api.Features.Notifications
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService notificationService;
        private readonly ILogger<NotificationsController> logger;

        public NotificationsController(INotificationService notificationService, ILogger<NotificationsController> logger)
        {
            this.notificationService = notificationService ?? throw new System.ArgumentNullException(nameof(notificationService));
            this.logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        // POST: api/notifications/subscribe
        [HttpPost]
        [Route("subscribe")]
        public async Task<ActionResult> Suscribe([FromBody] Subscriber subscriber, CancellationToken cancellationToken = default)
        {
            var response = await this.notificationService.SubscribeAsync(subscriber, cancellationToken);

            if (!response) return BadRequest();
            return Ok();
        }

        // POST: api/notifications/subscribe
        [HttpPost]
        [Route("unsubscribe")]
        public async Task<ActionResult> UnSuscribe([FromBody] Subscriber subscriber, CancellationToken cancellationToken = default)
        {
            var response = await this.notificationService.UnSubscribeAsync(subscriber, cancellationToken);

            if (!response) return BadRequest();
            return Ok();
        }

        // POST: api/notifications/sms
        [HttpPost]
        [Route("sms")]
        public async Task<ActionResult> SendSMS([FromBody] Message message, CancellationToken cancellationToken = default)
        {
            var response = await this.notificationService.SendAsync(message, cancellationToken);
            if(!response) return BadRequest();

            return Ok();
        }
    }
}