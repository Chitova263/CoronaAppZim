using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using CoronaAppZim.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoronaAppZim.Api.Features.Tracker
{
    [ApiController]
    [Route("api/[controller]")]
    public class Covid19TrackerController : ControllerBase
    {
        private readonly ICovidTrackerService covidTrackerService;
        private readonly ILogger<Covid19TrackerController> logger;

        public Covid19TrackerController(ICovidTrackerService covidTrackerService, ILogger<Covid19TrackerController> logger)
        {
            this.covidTrackerService = covidTrackerService ?? throw new ArgumentNullException(nameof(covidTrackerService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET: api/[controller]?country
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetLatestReport([FromQuery]string country, CancellationToken cancellationToken = default)
        {
            var response = await this.covidTrackerService.GetLatestReportAsync(country, cancellationToken);

            if(response.ResponseInfo.StatusCode != HttpStatusCode.OK)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
