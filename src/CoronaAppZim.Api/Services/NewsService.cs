using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CoronaAppZim.Api.Features.News;
using Microsoft.Extensions.Logging;

namespace CoronaAppZim.Api.Services
{
    public class NewsService: INewsService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILogger<NewsService> logger;

        public NewsService(IHttpClientFactory httpClientFactory, ILogger<NewsService> logger)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Stories> GetStoriesAsync(string query, CancellationToken cancellationToken = default)
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
                 $"https://newsapi.org/v2/everything?q={query}&apiKey=605925cd99af413c8e47e2a207d6abd6"
                );

            var client = httpClientFactory.CreateClient();

            this.logger.LogInformation("--- fetching news");

            var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

            if (response.IsSuccessStatusCode)
            {

               

                var contentString = await response.Content.ReadAsStringAsync();
                var story = Newtonsoft.Json.JsonConvert.DeserializeObject<Stories>(contentString);

                this.logger.LogInformation("--- fetching news successful");

                return story;
            }

            this.logger.LogInformation("--- fetching news failed");

            return null;
        }
    }
}
