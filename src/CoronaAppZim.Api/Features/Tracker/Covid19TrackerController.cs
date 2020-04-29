using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
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
            this.mediator = mediator;
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET: api/[controller]?country
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetLatestReport([FromQuery]GetLatestReportQuery.Request country, CancellationToken cancellationToken = default)
        {
            var response = await this.mediator.Send(country, cancellationToken);
            return Ok(response);
        }
    }
}
