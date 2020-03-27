using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Covid19.Api.Models
{
    [DataContract]
    public class Articles
    {
        [JsonProperty("articles")]
        public List<Article> ArticlesList { get; set; }
    }
}
