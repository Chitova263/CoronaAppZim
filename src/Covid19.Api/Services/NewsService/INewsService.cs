using System.Threading.Tasks;

namespace Covid19.Api.Services.NewsService
{
    public interface INewsService
    {
        Task<dynamic> GetAllNews(string query);
    }
}