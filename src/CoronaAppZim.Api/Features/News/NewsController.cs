using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoronaAppZim.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoronaAppZim.Api.Features.News
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly INewsService newsService;
        private readonly ILogger<NewsController> logger;

        public NewsController(INewsService newsService, ILogger<NewsController> logger)
        {
            this.newsService = newsService ?? throw new ArgumentNullException(nameof(newsService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET: api/[controller]?query
        [HttpGet]
        [Route("query")]
        public async Task<ActionResult> GetLatestNews([FromQuery]string query, CancellationToken cancellationToken = default)
        {
            this.logger.LogInformation("--- fetching news");

            var response = await this.newsService.GetStoriesAsync(query, cancellationToken);
            var result = response.Articles.OrderByDescending(x => x.PublishedAt);

            return Ok(result);
        }
    }
}
