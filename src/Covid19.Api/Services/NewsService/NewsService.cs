using Covid19.Api.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace Covid19.Api.Services.NewsService
{
    public class NewsService: INewsService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public NewsService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory ?? throw new System.ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<dynamic> GetAllNews(string query)
        {
            HttpClient httpClient = _httpClientFactory.CreateClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get,
                $"https://newsapi.org/v2/everything?q={query}&apiKey=605925cd99af413c8e47e2a207d6abd6"
                );

            var response = await httpClient.SendAsync(request);

            if(response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var json = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(content);
                return content;
            }

            return null;
        }
    }
}