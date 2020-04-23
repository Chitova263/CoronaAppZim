using System.Collections.Generic;
using Newtonsoft.Json;

namespace CoronaAppZim.Api.Features.News
{
    public class Stories
    {
        public List<Story> Articles { get; set; }
        [JsonProperty("source")]
        public Source ArticleSource { get; set; }

        public class Source
        {
            public string Name { get; set; }
        }
    }
}
