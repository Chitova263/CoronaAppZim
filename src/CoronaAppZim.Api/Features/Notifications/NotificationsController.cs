using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoronaAppZim.Api.Features.Notifications
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly ILogger<NotificationsController> logger;
        private readonly IMediator mediator;

        public NotificationsController(IMediator mediator, ILogger<NotificationsController> logger)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
            this.logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        // POST: api/notifications/subscribe
        [HttpPost]
        [Route("subscribe")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Suscribe([FromBody] SubscribeCommand.Command command, CancellationToken cancellationToken = default)
        {
            this.logger.LogInformation($"--- sending command @{typeof(SubscribeCommand)}");

            var commandResult = await this.mediator.Send(command, cancellationToken);

            if(!commandResult.IsSuccess)
            {
                return BadRequest(commandResult);
            }

            return Ok();
        }

        // POST: api/notifications/usubscribe
        [HttpPost]
        [Route("unsubscribe")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UnSuscribe([FromBody] UnSubscribeCommand.Command command, CancellationToken cancellationToken = default)
        {

            this.logger.LogInformation($"--- sending command @{typeof(UnSubscribeCommand)}");

            var commandResult = await this.mediator.Send(command, cancellationToken);

            if(!commandResult.IsSuccess)
            {
                return BadRequest(commandResult);
            }

            return Ok();
        }

        // POST: api/notifications/sms
        [HttpPost]
        [Route("sms")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> SendSMS([FromBody] SendSMSCommand.Command command, CancellationToken cancellationToken = default)
        {
            this.logger.LogInformation($"--- sending command @{typeof(SendSMSCommand)}");
            
            var commandResult = await this.mediator.Send(command, cancellationToken);
            
            if(!commandResult.IsSuccess)
            {
                return BadRequest(commandResult);
            }

            return Ok();
        }
    }
}