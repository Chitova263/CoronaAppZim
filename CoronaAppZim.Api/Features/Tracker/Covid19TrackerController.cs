using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoronaAppZim.Api.Features.Tracker
{
    [ApiController]
    [Route("api/[controller]")]
    public class Covid19TrackerController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly ILogger<Covid19TrackerController> logger;

        public Covid19TrackerController(IMediator mediator, ILogger<Covid19TrackerController> logger)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET: api/[controller]?country
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetLatestReport([FromQuery]GetLatestReportQuery.Request request, CancellationToken cancellationToken = default)
        {
            var response = await this.mediator.Send(request, cancellationToken);

            if (response.IsFailure)
                return BadRequest(response.FailureReason);


            return Ok(response.Value);
        }
    }
}
