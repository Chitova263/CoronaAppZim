using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoronaAppZim.Api.Features.News
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly ILogger<NewsController> logger;

        public NewsController(IMediator mediator, ILogger<NewsController> logger)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET: api/[controller]?query
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetLatestNews([FromQuery]GetStoriesQuery.Request query, CancellationToken cancellationToken = default)
        {
            this.logger.LogInformation("--- fetching news");

            var response = await this.mediator.Send(query, cancellationToken);
            var result = response.Articles.OrderByDescending(x => x.PublishedAt);

            return Ok(result);
        }
    }
}
