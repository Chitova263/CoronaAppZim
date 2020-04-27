using System;
using Newtonsoft.Json;

namespace CoronaAppZim.Api.Features.News
{
    public class Story
    {
        public Guid Id { get; set; }
        public string Author { get; set; }
        public string Title { get; set; } 
        public string Description { get; set; }
        public string Url { get; set; }
        public string UrlToImage { get; set }
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
