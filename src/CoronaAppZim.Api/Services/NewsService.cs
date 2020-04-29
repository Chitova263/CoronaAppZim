using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CoronaAppZim.Api.Config;
using CoronaAppZim.Api.Features.News;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CoronaAppZim.Api.Services
{
    public class NewsService: INewsService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILogger<NewsService> logger;
        private readonly IOptionsMonitor<NewsApiSettings> options;

        public NewsService(IHttpClientFactory httpClientFactory, ILogger<NewsService> logger, IOptionsMonitor<NewsApiSettings> options)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task<Stories> GetStoriesAsync(string query, CancellationToken cancellationToken = default)
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
                 $"https://newsapi.org/v2/everything?q={query}&apiKey={this.options.CurrentValue.ApiKey}"
                );

            var client = httpClientFactory.CreateClient();

            this.logger.LogInformation("--- fetching news");

            var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var contentStream = await response.Content.ReadAsStreamAsync();

                //read the stream using a stream reader
                using (var streamReader = new StreamReader(contentStream, Encoding.UTF8))
                using (var textReader = new JsonTextReader(streamReader))
                {
                    var jsonSerializer = new JsonSerializer();
                    var stories = jsonSerializer.Deserialize<Stories>(textReader);
                    return stories;
                }
            }

            return null;
        }
    }
}
