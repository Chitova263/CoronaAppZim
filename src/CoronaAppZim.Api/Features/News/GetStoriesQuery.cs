using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CoronaAppZim.Api.Configuration;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CoronaAppZim.Api.Features.News
{
    public class GetStoriesQuery
    {
       public class Request: IRequest<Response>
       {
           public string Query { get; set; }
       }

        public class Response
        {
            public List<Story> Articles { get; set; }
            [JsonProperty("source")]
            public Source ArticleSource { get; set; }

            public class Source
            {
                public string Name { get; set; }
            }

            public class Story
            {
                public Guid Id { get; set; }
                public string Author { get; set; }
                public string Title { get; set; } 
                public string Description { get; set; }
                public string Url { get; set; }
                public string UrlToImage { get; set; }
                public DateTimeOffset PublishedAt { get; set; }

                [JsonProperty("source")]
                public Source ArticleSource { get; set; }

                public class Source
                {
                    public string Name { get; set; }
                }

                protected Story()
                {
                    Id = Guid.NewGuid();
                }
            }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IHttpClientFactory httpClientFactory;
            private readonly IOptions<NewsApiSettings> options;
            private readonly ILogger<Handler> logger;

            public Handler(IHttpClientFactory httpClientFactory, IOptions<NewsApiSettings> options, ILogger<Handler> logger)
            {
                this.httpClientFactory = httpClientFactory ?? throw new System.ArgumentNullException(nameof(httpClientFactory));
                this.options = options ?? throw new System.ArgumentNullException(nameof(options));
                this.logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
            }
            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Get,
                    $"https://newsapi.org/v2/everything?q={request.Query}&apiKey={this.options.Value.ApiKey}"
                );

                var client = httpClientFactory.CreateClient();

                this.logger.LogInformation("--- fetching news");

                var response = await client.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    var contentStream = await response.Content.ReadAsStreamAsync();

                    //read the stream using a stream reader
                    using (var streamReader = new StreamReader(contentStream, Encoding.UTF8))
                    using (var textReader = new JsonTextReader(streamReader))
                    {
                        var jsonSerializer = new JsonSerializer();
                        var stories = jsonSerializer.Deserialize<Response>(textReader);
                        return stories;
                    }
                }

                return null;
            }
        }
    }

   
}